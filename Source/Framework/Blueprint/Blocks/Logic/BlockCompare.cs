using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OpenGLF
{
    public enum CompareType { Greater, Less, Equal, NotEqual, GreaterOrEqual, LessOrEqual }

    [Serializable]
    public class BlockCompare : BlockFunction
    {
        CompareType _compareType = CompareType.Equal;
        public CompareType compareType
        {
            get
            {
                return _compareType;
            }
            set
            {
                _compareType = value;

                switch (_compareType)
                {
                    case CompareType.Equal:
                        text = "A = B";
                        break;
                    case CompareType.Greater:
                        text = "A > B";
                        break;
                    case CompareType.GreaterOrEqual:
                        text = "A >= B";
                        break;
                    case CompareType.Less:
                        text = "A < B";
                        break;
                    case CompareType.LessOrEqual:
                        text = "A <= B";
                        break;
                    case CompareType.NotEqual:
                        text = "A != B";
                        break;
                }
            }
        }

        Connector c_obj1 = null;
        Connector c_obj2 = null;

        Connector c_out2 = null;

        public BlockCompare(Blueprint owner) : base(owner)
        {
            compareType = CompareType.Equal;

            c_obj1 = new Connector(this, ConnectorType.Input);
            c_obj1.name = "Value 1";
            c_obj1.value = 0;

            c_obj2 = new Connector(this, ConnectorType.Input);
            c_obj2.name = "Value 2";
            c_obj2.value = 0;

            c_out.name = "True";

            c_out2 = new Connector(this, ConnectorType.Output);
            c_out2.name = "False";
            c_out2.color = Color.red;
            c_out2.value = new ConnectorDummy();
        }

        public override void doFunction()
        {
            try {
                float o1 = 0;
                float o2 = 0;

                if (c_obj1.link != null)
                {
                    switch (c_obj1.link.value.GetType().Name)
                    {
                        case "Int32":
                            o1 = (int)c_obj1.link.value;
                            break;
                        case "Float":
                            o1 = (float)c_obj1.link.value;
                            break;
                    }
                    
                }

                if (c_obj2.link != null)
                {
                    switch (c_obj2.link.value.GetType().Name)
                    {
                        case "Int32":
                            o2 = (int)c_obj2.link.value;
                            break;
                        case "Float":
                            o2 = (float)c_obj2.link.value;
                            break;
                    }
                }

                switch (compareType)
                {
                    case CompareType.Equal:
                        if (o1 == o2)
                            doNext(c_out);
                        else
                            doNext(c_out2);
                        break;
                    case CompareType.Greater:
                        if (o1 > o2)
                            doNext(c_out);
                        else
                            doNext(c_out2);
                        break;
                    case CompareType.GreaterOrEqual:
                        if (o1 >= o2)
                            doNext(c_out);
                        else
                            doNext(c_out2);
                        break;
                    case CompareType.Less:
                        if (o1 < o2)
                            doNext(c_out);
                        else
                            doNext(c_out2);
                        break;
                    case CompareType.LessOrEqual:
                        if (o1 <= o2)
                            doNext(c_out);
                        else
                            doNext(c_out2);
                        break;
                    case CompareType.NotEqual:
                        if (o1 != o2)
                            doNext(c_out);
                        else
                            doNext(c_out2);
                        break;
                }
            }
            catch { }
        }
    }
}
