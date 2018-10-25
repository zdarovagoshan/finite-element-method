// Quick and dirty OpenGL 4.x <-> OpenCL interop test

#include <stdio.h>
#include <stdlib.h>
#include <memory.h>

#include <GL/glew.h>
#include <GL/glxew.h>

void init_glx(void)
{
	glXChooseFBConfig =
		(PFNGLXCHOOSEFBCONFIGPROC)
		glXGetProcAddress((GLubyte*)"glXChooseFBConfig");
	glXGetVisualFromFBConfig =
		(PFNGLXGETVISUALFROMFBCONFIGPROC)
		glXGetProcAddress((GLubyte*)"glXGetVisualFromFBConfig");
	glXCreateContextAttribsARB =
		(PFNGLXCREATECONTEXTATTRIBSARBPROC)
		glXGetProcAddress((GLubyte*)"glXCreateContextAttribsARB");
}

void init_gl(Display* disp, int w, int h,
						 Window* out_win, GLXContext* out_ctx)
{
	static int fb_attribs[] = {
		GLX_RENDER_TYPE, GLX_RGBA_BIT,
		GLX_X_RENDERABLE, True,
		GLX_DRAWABLE_TYPE, GLX_WINDOW_BIT,
		GLX_DOUBLEBUFFER, True,
		GLX_RED_SIZE, 8,
		GLX_BLUE_SIZE, 8,
		GLX_GREEN_SIZE, 8,
		0
	};

	static GLint gl_attribs[] = {
		GLX_CONTEXT_MAJOR_VERSION_ARB, 4,
		GLX_CONTEXT_MINOR_VERSION_ARB, 0,
		GLX_CONTEXT_PROFILE_MASK_ARB, GLX_CONTEXT_CORE_PROFILE_BIT_ARB,
		0
	};

	Window win;
	XVisualInfo* visual_info;
	XSetWindowAttributes attribs;

	int num_configs = 0;
	GLXFBConfig* fb_configs;
	GLXContext ctx;

	memset(&attribs, 0, sizeof(XSetWindowAttributes));

	fb_configs  = glXChooseFBConfig(disp, DefaultScreen(disp), fb_attribs, &num_configs);
	visual_info = glXGetVisualFromFBConfig(disp, fb_configs[0]);

	attribs.event_mask = ExposureMask | VisibilityChangeMask |
		KeyPressMask | PointerMotionMask | StructureNotifyMask;
	
	attribs.border_pixel = 0;
	attribs.bit_gravity  = StaticGravity;
	attribs.colormap     = XCreateColormap(disp, RootWindow(disp, visual_info->screen),
																				 visual_info->visual, AllocNone);

	win = XCreateWindow(disp, DefaultRootWindow(disp), 0, 0, w, h, 0,
											visual_info->depth, InputOutput, visual_info->visual,
											CWBorderPixel | CWBitGravity | CWEventMask | CWColormap,
											&attribs);
	XMapWindow(disp, win);

	ctx = glXCreateContextAttribsARB(disp, fb_configs[0], 0, True, gl_attribs);
	glXMakeCurrent(disp, win, ctx);

	XFlush(disp);
	XFree(fb_configs);
	XFree(visual_info);
	
	glewInit();

	*out_win = win;
	*out_ctx = ctx;
}

void destroy_gl(Display* disp, Window win, GLXContext ctx)
{
	glXMakeCurrent(disp, None, NULL);
	glXDestroyContext(disp, ctx);
	XDestroyWindow(disp, win);
}

void draw(void)
{
	glViewport(0, 0, 800, 600);
	glClearColor(1.0f, 0.0f, 0.0f, 1.0f);
	glClear(GL_COLOR_BUFFER_BIT);
}

int main()
{
	Display *disp = XOpenDisplay(NULL);
	Window win;
	GLXContext ctx;
	
	int done = False;

	init_glx();
	init_gl(disp, 800, 600, &win, &ctx);
	printf("OpenGL version = %s\n", glGetString(GL_VERSION));
	
	while(1) {
		XEvent ev;
		XNextEvent(disp, &ev);

		switch(ev.type) {
		case KeyPress:
			if(XLookupKeysym(&ev.xkey, 0) == XK_Escape)
				done = True;
			break;
		case DestroyNotify:
			done = True;
			break;
		}
		if(done == True) break;
		
		draw();
		glXSwapBuffers(disp, win);
	}

	destroy_gl(disp, win, ctx);
	XCloseDisplay(disp);
	return 0;
}