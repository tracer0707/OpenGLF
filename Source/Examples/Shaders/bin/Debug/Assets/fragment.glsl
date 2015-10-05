varying vec3 N;
varying vec3 v;
varying vec3 lightVec[10];

uniform vec4 color;
uniform vec4 ambient;
varying vec4 vTexCoord;
uniform vec4 light_p[10];
uniform vec4 light_col[10];
uniform float light_int[10];

uniform float amount; // Множитель рельефа
uniform int light_c;

uniform sampler2D normal; // Карта высот
uniform sampler2D texture; // текстура

void main(void)
{
   vec4 base = texture2D(texture, vTexCoord);
   vec3 bump = normalize(texture2D(normal, vTexCoord).xyz * 2.0 - 1.0) * 4;
   vec4 vAmbient = ambient;
   vec4 col = vec4(0,0,0,1);

   for (int i = 0; i < light_c; i++)
   {
		float distSqr = dot(lightVec[i], lightVec[i]);
		float att = clamp(1.0 * sqrt(distSqr), 0.0, 1.0);
		vec3 lVec = lightVec[i] * inversesqrt(distSqr);
		vec3 vVec = normalize(light_p[i]);
		
		float diffuse = max( dot(lVec, bump), 0.0 ) * amount;
		vec4 vDiffuse = light_col[i] * diffuse * light_int[i];    
		col += (vAmbient * base + vDiffuse * base) * att;
   }

   gl_FragColor = col;
}