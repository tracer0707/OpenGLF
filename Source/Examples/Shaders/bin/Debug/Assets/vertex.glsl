varying vec3 N;
varying vec3 v;
varying vec4 vTexCoord;
attribute vec3 vTangent;
uniform vec4 light_p[10];
varying vec3 lightVec[10];
uniform int light_c;

void main(void)
{
   gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
   v = vec3(gl_ModelViewMatrix * gl_Vertex);
   N = normalize(gl_NormalMatrix * gl_Normal);
   
   vTexCoord = gl_MultiTexCoord0;
   
   vec3 n = normalize(gl_NormalMatrix * gl_Normal);
   vec3 t = normalize(gl_NormalMatrix * vTangent);
   vec3 b = cross(n, t);
   
   vec3 vVertex = vec3(gl_ModelViewMatrix * gl_Vertex);

   for (int i = 0; i < light_c; i++)
   {
		lightVec[i].x = dot(light_p[i].xyz - vVertex, t);
		lightVec[i].y = dot(light_p[i].xyz - vVertex, b);
		lightVec[i].z = dot(light_p[i].xyz - vVertex, n);
   }
}