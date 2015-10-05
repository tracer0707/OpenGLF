using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarseerPhysics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Common.ConvexHull;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common.PhysicsLogic;
using FarseerPhysics.Common.PolygonManipulation;
using FarseerPhysics.Common.TextureTools;
using FarseerPhysics.Controllers;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing.Design;
using OpenTK.Graphics.OpenGL;
using System.IO;
using System.Drawing.Imaging;

namespace OpenGLF
{
    [Serializable]
    public class Rigidbody : Component
    {
        List<Vector> _verts = new List<Vector>();
        OpenGLF.BodyType _bodyType = BodyType.Dynamic;
        float _angularDamping = 0.1f;
        float _angularVelocity = 0;
        bool _enabled = true;
        bool _fixedRotation = false;
        float _friction = 1;
        float _linearDamping = 0.1f;
        float _mass = 1;
        float _restitution = 1;
        float _rotation = 0;
        Vector _linearVelocity;
        Vector _localCenter;

        [NonSerialized]
        internal Body body;

        [NonSerialized]
        internal List<Fixture> fixture;

        [NonSerialized]
        internal List<Vertices> _vertices;

        [Editor(typeof(VerticesEditor), typeof(UITypeEditor))]
        public List<Vector> vertices { get { return _verts; } internal set { _verts = value; } }

        public OpenGLF.BodyType bodyType
        {
            get
            {
                return _bodyType;
            }

            set
            {
                _bodyType = value;
                setBodyType(value);
            }
        }

        public float angularDamping
        {
            get { return _angularDamping; }
            set { _angularDamping = value; if (body != null) body.AngularDamping = value; }
        }

        [Browsable(false)]
        public float angularVelocity
        {
            get { return _angularVelocity; }
            set { _angularVelocity = value; if (body != null) body.AngularVelocity = value; }
        }

        public bool bodyEnabled
        {
            get { return _enabled; }
            set { _enabled = value; if (body != null) body.Enabled = value; }
        }

        public bool fixedRotation
        {
            get { return _fixedRotation; }
            set { _fixedRotation = value; if (body != null) body.FixedRotation = value; }
        }

        public float friction
        {
            get { return _friction; }
            set { _friction = value; if (body != null) body.Friction = value; }
        }

        public float linearDamping
        {
            get { return _linearDamping; }
            set { _linearDamping = value; if (body != null) body.LinearDamping = value; }
        }

        [Browsable(false)]
        public Vector linearVelocity
        {
            get { if (body != null) return new Vector(body.LinearVelocity.X, body.LinearVelocity.Y); else return _linearVelocity; }
            set { _linearVelocity = value; if (body != null) body.LinearVelocity = new Vector2((float)value.x, (float)value.y); }
        }

        [Browsable(false)]
        public Vector localCenter
        {
            get { return _localCenter; }
            set { _localCenter = value; if (body != null) body.LocalCenter = new Vector2((float)value.x, (float)value.y); }
        }

        [Browsable(false)]
        public Vector position
        {
            get { return new Vector(body.Position.X, body.Position.Y); }
            set { if (body != null) body.Position = new Vector2((float)value.x, (float)value.y); }
        }

        [Browsable(false)]
        public float mass
        {
            get { return _mass; }
            set { _mass = value; if (body != null) body.Mass = value; }
        }

        [Browsable(false)]
        public float restitution
        {
            get { return _restitution; }
            set { _restitution = value; if (body != null) body.Restitution = value; }
        }

        [Browsable(false)]
        public float rotation
        {
            get { return _rotation; }
            set { _rotation = value; if (body != null) body.Rotation = value; }
        }

        public delegate void OnCollision(GameObject collider);

        public OnCollision onCollision;

        public Rigidbody()
        {
            vertices = new List<Vector>();
        }

        internal override void start()
        {
            float angle = (float)gameObject.angle;
            
            if (vertices.Count <= 1)
            {
                vertices.Add(new Vector(0, 0));
                vertices.Add(new Vector(10, 0));
                vertices.Add(new Vector(10, 10));
                vertices.Add(new Vector(0, 10));
            }

            Vertices vrts = new Vertices();
            for (int i = 0; i < vertices.Count; i++)
            {
                vrts.Add(new Vector2((float)vertices[i].x, (float)vertices[i].y));
            }

            _vertices = FarseerPhysics.Common.Decomposition.Triangulate.ConvexPartition(vrts, TriangulationAlgorithm.Bayazit);

            body = new Body(Engine.world, new Vector2((float)gameObject.position.x, (float)gameObject.position.y), (float)Mathf.toRadians(angle), FarseerPhysics.Dynamics.BodyType.Dynamic, null);
            setBodyType(bodyType);
            fixture = FixtureFactory.AttachCompoundPolygon(_vertices, 1.0f, body);

            body.AngularDamping = _angularDamping;
            body.Enabled = _enabled;
            body.FixedRotation = _fixedRotation;
            body.Friction = _friction;
            body.LinearDamping = _linearDamping;

            body.OnCollision += body_OnCollision;
        }

        bool body_OnCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if (onCollision != null)
            {
                GameObject col = null;
                for (int i = 0; i < Engine.scene.objects.Count; i++)
                {
                    if (Engine.scene.objects[i].rigidbody != null)
                    for (int j = 0; j < Engine.scene.objects[i].rigidbody.fixture.Count; j++)
                        if (Engine.scene.objects[i].rigidbody.fixture[j] == fixtureB)
                            col = Engine.scene.objects[i];
                }
                
                onCollision(col);
            }
            return true;
        }

        internal override void update()
        {
            gameObject.position = new Vector(body.Position.X, body.Position.Y);
            gameObject.angle = (float)Mathf.toDegrees(body.Rotation);
        }

        internal override void draw(OpenTK.Graphics.OpenGL.RenderingMode mode)
        {
            if (Engine.debugPhysics == true)
            {
                if (mode == OpenTK.Graphics.OpenGL.RenderingMode.Render)
                {
                    GL.PushMatrix();
                    GL.Translate(gameObject.position.x, gameObject.position.y, 0);

                    GL.Rotate(gameObject.angle, 0, 0, 1);

                    if (_vertices != null)
                    {
                        for (int i = 0; i < _vertices.Count; i++)
                        {
                            Vertices v = _vertices[i];

                            if (v.Count > 2)
                            {
                                for (int j = 0; j < v.Count - 1; j++)
                                    Drawing.drawLine(new Vector(v[j].X, v[j].Y), new Vector(v[j + 1].X, v[j + 1].Y), 1f, Color.green);

                                Drawing.drawLine(new Vector(v[v.Count - 1].X, v[v.Count - 1].Y), new Vector(v[0].X, v[0].Y), 1f, Color.green);
                            }
                        }
                    }
                    else if (vertices.Count > 2)
                    {
                        for (int j = 0; j < vertices.Count - 1; j++)
                        {
                            Drawing.drawLine(vertices[j], vertices[j + 1], 1f, Color.green);
                        }

                        Drawing.drawLine(vertices[vertices.Count - 1], vertices[0], 1f, Color.green);
                    }

                    GL.PopMatrix();
                }
            }
        }

        void setBodyType(BodyType value)
        {
            if (body != null)
            {
                switch (value)
                {
                    case BodyType.Dynamic:
                        body.BodyType = FarseerPhysics.Dynamics.BodyType.Dynamic;
                        break;
                    case BodyType.Static:
                        body.BodyType = FarseerPhysics.Dynamics.BodyType.Static;
                        break;
                    case BodyType.Kinematic:
                        body.BodyType = FarseerPhysics.Dynamics.BodyType.Kinematic;
                        break;
                }
            }
        }

        public override Component clone()
        {
            Rigidbody rb = new Rigidbody();
            rb.bodyType = bodyType;
            rb.angularDamping = angularDamping;
            rb.angularVelocity = angularVelocity;
            rb.enabled = enabled;
            rb.fixedRotation = fixedRotation;
            rb.friction = friction;
            rb.linearDamping = linearDamping;
            rb.linearVelocity = linearVelocity;
            rb.localCenter = localCenter.clone();
            rb.mass = mass;
            rb.restitution = restitution;

            for (int i = 0; i < vertices.Count; i++)
            {
                rb.vertices.Add(vertices[i].clone());
            }

            return rb;
        }

        public void shapeFromTexture(Texture tex)
        {
            byte[] data;
            

            using (MemoryStream ms = new MemoryStream())
            {
                tex.bitmap.Save(ms, ImageFormat.Png);
                data = ms.ToArray();
                ms.Dispose();
            }

            uint[] bytes = new uint[data.Length];

            for (int i = 0; i < data.Length; i++)
                bytes[i] = data[i];

            Vertices verts = PolygonTools.CreatePolygon(bytes, tex.bitmap.Width, true);

            vertices.Clear();

            for (int i = 0; i < verts.Count; i++)
            {
                vertices.Add(new Vector((float)verts[i].X, (float)verts[i].Y));
            }
        }

        public void addForce(Vector force, Vector point)
        {
            if (body != null)
            {
                body.ApplyForce(new Vector2(force.x, force.y), new Vector2(point.x, point.y));
            }
        }

        public bool raycast(Vector ray)
        {
            Vector2 point1 = new Vector2(position.x, position.y);
            Vector2 point2 = new Vector2(ray.x, ray.y);

            bool hitClosest = false;
            Vector2 point = Vector2.Zero, normal = Vector2.Zero;

            Engine.world.RayCast((f, p, n, fr) =>
                                      {
                                          Body body = f.Body;
                                          if (body.UserData != null)
                                          {
                                              int index = (int)body.UserData;
                                              if (index == 0)
                                              {
                                                  // filter
                                                  return -1.0f;
                                              }
                                          }

                                          hitClosest = true;
                                          point = p;
                                          normal = n;
                                          return fr;
                                      }, point1, point2);
            if (hitClosest)
                return true;
            else
                return false;
                
        }

        [OnDeserializedAttribute()]
        private void onDeserialized(StreamingContext context)
        {
            if (_verts == null)
                _verts = new List<Vector>();
        }
    }

    [Serializable]
    public enum BodyType
    {
        Dynamic = 1,
        Static = 2,
        Kinematic = 3
    }
}
