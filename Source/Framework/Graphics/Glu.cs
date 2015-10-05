using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGLF
{

public class Glu
{

static public int intersect_RayTriangle( Vector3d RP0, Vector3d RP1,
		   	Vector3 TV0, Vector3 TV1, Vector3 TV2,
		    ref Vector3 I )
		{
            Vector3 _RP0 = new Vector3((float)RP0.X, (float)RP0.Y, (float)RP0.Z);
            Vector3 _RP1 = new Vector3((float)RP1.X, (float)RP1.Y, (float)RP1.Z); 
			return intersect_RayTriangle(_RP0, _RP1, TV0, TV1, TV2, ref I);
		}
		
static double SMALL_NUM = 0.00000001;

// intersect_RayTriangle(): intersect a ray with a 3D triangle
//    Input:  a ray R, and a triangle T
//    Output: *I = intersection point (when it exists)
//    Return: -1 = triangle is degenerate (a segment or point)
//             0 = disjoint (no intersect)
//             1 = intersect in unique point I
//			   2 = intersect in unique point I, from back face of triangle
//             3 = are in the same plane
static public int intersect_RayTriangle( Vector3 RP0, Vector3 RP1,
		   	Vector3 TV0, Vector3 TV1, Vector3 TV2,
		    ref Vector3 I )
{
    Vector3   u, v, n;             // triangle vectors
    Vector3   dir, w0, w;          // ray vectors
    double     r, a, b;             // params to calc ray-plane intersect

    // get triangle edge vectors and plane normal
    u = TV1 - TV0;
    v = TV2 - TV0;
    n = Vector3.Cross(u, v);             // cross product
    if (n == new Vector3(0,0,0))            // triangle is degenerate
        return -1;                 // do not deal with this case

    dir = RP1 - RP0;             // ray direction vector
    w0 = RP0 - TV0;
    a = -Vector3.Dot(n,w0);
    b = Vector3.Dot(n,dir);
    if (Math.Abs(b) < SMALL_NUM) {     // ray is parallel to triangle plane
        if (a == 0)                // ray lies in triangle plane
            return 3;
        else return 0;             // ray disjoint from plane
    }

    // get intersect point of ray with triangle plane
    r = a / b;
    if (r < 0.0F)                   // ray goes away from triangle
        return 0;                  // => no intersect
    // for a segment, also test if (r > 1.0) => no intersect

    I = RP0 + (float)r * dir;           // intersect point of ray and plane		

    // is I inside T?
    double    uu, uv, vv, wu, wv, D;
    uu = Vector3.Dot(u,u);
    uv = Vector3.Dot(u,v);
    vv = Vector3.Dot(v,v);
    w = I - TV0;
    wu = Vector3.Dot(w,u);
    wv = Vector3.Dot(w,v);
    D = uv * uv - uu * vv;

    // get and test parametric coords
    double s, t;
    s = (uv * wv - vv * wu) / D;
    if (s < 0.0F || s > 1.0F)        // I is outside T
        return 0;
    t = (uv * wu - uu * wv) / D;
    if (t < 0.0F || (s + t) > 1.0F)  // I is outside T
        return 0;

	if (b < 0) return 2;		   // I is in T, intersecting from back face
    return 1;                      // I is in T
}

		
	static double __glPi = 3.14159265358979323846;

	public static void gluPerspective (double fovy, double aspect, double zNear, double zFar)
	{
		Matrix4d m = Matrix4d.Identity;
		double sine, cotangent, deltaZ;
		double radians = fovy / 2 * __glPi / 180;
		
		deltaZ = zFar - zNear;
		sine = Math.Sin (radians);
		if ((deltaZ == 0) || (sine == 0) || (aspect == 0)) {
			return;
		}
		//TODO: check why this cos was written COS?
		cotangent = Math.Cos (radians) / sine;
		
		m.M11 = cotangent / aspect;
		m.M22 = cotangent;
		m.M33 = -(zFar + zNear) / deltaZ;
		m.M34 = -1;
		m.M43 = -2 * zNear * zFar / deltaZ;
		m.M44 = 0;
		
		GL.MultMatrix (ref m);
	}


	public void LookAt (Vector3 eye, Vector3 center, Vector3 up)
	{
		gluLookAt (eye.X, eye.Y, eye.Z, center.X, center.Y, center.Z, up.X, up.Y, up.Z);
	}

	void gluLookAt (double eyex, double eyey, double eyez, double centerx, double centery, double centerz, double upx, double upy, double upz)
	{
		Vector3 forward, side, up;
		Matrix4 m;
		
		forward.X = (float)(centerx - eyex);
        forward.Y = (float)(centery - eyey);
        forward.Z = (float)(centerz - eyez);

        up.X = (float)upx;
        up.Y = (float)upy;
        up.Z = (float)upz;
		
		forward.Normalize ();
		
		/* Side = forward x up */
		side = Vector3.Cross (forward, up);
		side.Normalize ();
		
		/* Recompute up as: up = side x forward */
		up = Vector3.Cross (side, forward);
		
		m = Matrix4.Identity;
		
		m.M11 = side.X;
		m.M21 = side.Y;
		m.M31 = side.Z;
		
		m.M12 = up.X;
		m.M22 = up.Y;
		m.M32 = up.Z;
		
		m.M13 = -forward.X;
		m.M23 = -forward.Y;
		m.M33 = -forward.Z;
		
		GL.MultMatrix (ref m);
		GL.Translate (-eyex, -eyey, -eyez);
	}


	public int Project (Vector3d obj, ref Vector3d win)
	{
		Matrix4d modelMatrix;
		GL.GetDouble (GetPName.ModelviewMatrix, out modelMatrix);
		
		Matrix4d projMatrix;
		GL.GetDouble (GetPName.ProjectionMatrix, out projMatrix);
		
		int[] viewport = new int[4];
		GL.GetInteger (GetPName.Viewport, viewport);
		
		return Project (obj, modelMatrix, projMatrix, viewport, ref win);
	}

	public int Project (Vector3d obj, Matrix4d modelMatrix, Matrix4d projMatrix, int[] viewport, ref Vector3d win)
	{
        return gluProject((double)obj.X, (double)obj.Y, (double)obj.Z, modelMatrix, projMatrix, viewport, ref win.X, ref win.Y, ref win.Z);
	}

	int gluProject (double objx, double objy, double objz, Matrix4d modelMatrix, Matrix4d projMatrix, int[] viewport, ref double winx, ref double winy, ref double winz)
	{
		Vector4d _in;
		Vector4d _out;
		
		_in.X = objx;
		_in.Y = objy;
		_in.Z = objz;
		_in.W = 1.0;
    	//__gluMultMatrixVecd(modelMatrix, in, out);
	    //__gluMultMatrixVecd(projMatrix, out, in);
		//TODO: check if multiplication is in right order
		_out = Vector4d.Transform (_in, modelMatrix);
		_in = Vector4d.Transform (_out, projMatrix);
		
		if (_in.W == 0.0)
			return (0);
		_in.X /= _in.W;
		_in.Y /= _in.W;
		_in.Z /= _in.W;
		/* Map x, y and z to range 0-1 */		
		_in.X = _in.X * 0.5 + 0.5;
		_in.Y = _in.Y * 0.5 + 0.5;
		_in.Z = _in.Z * 0.5 + 0.5;
		
		/* Map x,y to viewport */
		_in.X = _in.X * viewport[2] + viewport[0];
		_in.Y = _in.Y * viewport[3] + viewport[1];
		
		winx = _in.X;
		winy = _in.Y;
		winz = _in.Z;
		return (1);
	}

	static public int UnProject (Vector3d win, ref Vector3d obj)
	{
		Matrix4d modelMatrix;
		GL.GetDouble (GetPName.ModelviewMatrix, out modelMatrix);
		
		Matrix4d projMatrix;
		GL.GetDouble (GetPName.ProjectionMatrix, out projMatrix);
		
		int[] viewport = new int[4];
		GL.GetInteger (GetPName.Viewport, viewport);
		
		return UnProject (win, modelMatrix, projMatrix, viewport, ref obj);
	}

	static public int UnProject (Vector3d win, Matrix4d modelMatrix, Matrix4d projMatrix, int[] viewport, ref Vector3d obj)
	{
		return gluUnProject (win.X, win.Y, win.Z, modelMatrix, projMatrix, viewport, ref obj.X, ref obj.Y, ref obj.Z);
	}

	static int gluUnProject (double winx, double winy, double winz, Matrix4d modelMatrix, Matrix4d projMatrix, int[] viewport, ref double objx, ref double objy, ref double objz)
	{
		Matrix4d finalMatrix;
		Vector4d _in;
		Vector4d _out;
		
		finalMatrix = Matrix4d.Mult (modelMatrix, projMatrix);
		
		//if (!__gluInvertMatrixd(finalMatrix, finalMatrix)) return(GL_FALSE);
		finalMatrix.Invert ();
		
		_in.X = winx;
		_in.Y = winy;
		_in.Z = winz;
		_in.W = 1.0;
		
		/* Map x and y from window coordinates */
		_in.X = (_in.X - viewport[0]) / viewport[2];
		_in.Y = (_in.Y - viewport[1]) / viewport[3];
		
		/* Map to range -1 to 1 */
		_in.X = _in.X * 2 - 1;
		_in.Y = _in.Y * 2 - 1;
		_in.Z = _in.Z * 2 - 1;
		
		//__gluMultMatrixVecd(finalMatrix, _in, _out);
		// check if this works:
		_out = Vector4d.Transform (_in, finalMatrix);
		
		if (_out.W == 0.0)
			return (0);
		_out.X /= _out.W;
		_out.Y /= _out.W;
		_out.Z /= _out.W;
		objx = _out.X;
		objy = _out.Y;
		objz = _out.Z;
		return (1);
	}

	int gluUnProject4 (double winx, double winy, double winz, double clipw, Matrix4d modelMatrix, Matrix4d projMatrix, int[] viewport, double near, double far, ref double objx,
	ref double objy, ref double objz, ref double objw)
	{
		Matrix4d finalMatrix;
		Vector4d _in;
		Vector4d _out;
		
		finalMatrix = Matrix4d.Mult (modelMatrix, projMatrix);
		
		//if (!__gluInvertMatrixd(finalMatrix, finalMatrix)) return(GL_FALSE);
		finalMatrix.Invert ();
		
		_in.X = winx;
		_in.Y = winy;
		_in.Z = winz;
		_in.W = clipw;
		
		/* Map x and y from window coordinates */
		_in.X = (_in.X - viewport[0]) / viewport[2];
		_in.Y = (_in.Y - viewport[1]) / viewport[3];
		_in.Z = (_in.Z - near) / (far - near);
		
		/* Map to range -1 to 1 */
		_in.X = _in.X * 2 - 1;
		_in.Y = _in.Y * 2 - 1;
		_in.Z = _in.Z * 2 - 1;
		
		// TODO: check again (same order issue as prev. todos)
		_out = Vector4d.Transform (_in, finalMatrix);
		if (_out.W == 0.0)
			return (0);
		
		objx = _out.X;
		objy = _out.Y;
		objz = _out.Z;
		objw = _out.W;
		return (1);
	}

	public static void gluPickMatrix (double x, double y, double deltax, double deltay, int[] viewport)
	{
		if (deltax <= 0 || deltay <= 0) {
			return;
		}
		
		/* Translate and scale the picked region to the entire window */
 		GL.Translate ((viewport[2] - 2 * (x - viewport[0])) / deltax, (viewport[3] - 2 * (y - viewport[1])) / deltay, 0);
    	//glScalef(viewport[2] / deltax, viewport[3] / deltay, 1.0);
		GL.Scale (viewport[2] / deltax, viewport[3] / deltay, 1.0);
	}
	
}

}
