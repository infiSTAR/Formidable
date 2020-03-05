using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Formidable.Util
{

    public static class MathUtil
    {

        static MathUtil()
        {

        }

        public static float Map(float value, float sourceFrom, float sourceTo, float destinationFrom, float destinationTo)
        {
            return ((value - sourceFrom) / (sourceTo - sourceFrom) * (destinationTo - destinationFrom) + destinationFrom);
        }

    }

}
