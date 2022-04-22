using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel;
using System.Diagnostics;

namespace webCRUD.Utilities
{
    public partial class Utilities
    {
        public DataTable ConvertListToDataTable<T>(IList<T> data)
        {
            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(T));
            DataTable dt = new DataTable();

            foreach (PropertyDescriptor property in properties)
                dt.Columns.Add(property.Name, Nullable.GetUnderlyingType(property.PropertyType) ?? property.PropertyType);

            foreach (T item in data)
            {
                DataRow dr = dt.NewRow();
                foreach (PropertyDescriptor property in properties)
                    dr[property.Name] = property.GetValue(item) ?? DBNull.Value;

                dt.Rows.Add(dr);
            }

            return dt;
        }

        public string GenerateSourceTrace(StackTrace st, StackTrace stError, System.Type type)
        {
            string trace = string.Empty;

            if (stError != null && stError.FrameCount > 0)
            {
                for (int a = 0; a < stError.FrameCount; a++)
                {
                    trace += @"·|ERROR·|: " + stError.GetFrame(a).GetMethod().ReflectedType.FullName + @"." + stError.GetFrame(a).GetMethod().Name + @".";

                    if (stError.GetFrame(a).GetFileName() != null)
                        trace += @" ·|LINE·|: " + stError.GetFrame(a).GetFileLineNumber() + @".";

                    trace += "\r\n";
                }
            }

            if (st != null && st.FrameCount > 0)
            {
                for (int a = 0; a < st.FrameCount; a++)
                {
                    if (st.GetFrame(a).GetFileName() != null)
                    {
                        if (st.GetFrame(a).GetMethod().DeclaringType != type && !type.IsAssignableFrom(st.GetFrame(a).GetMethod().DeclaringType))
                        {
                            trace += @"·|ERROR·|: " + st.GetFrame(a).GetMethod().ReflectedType.FullName + @"." + st.GetFrame(a).GetMethod().Name + @".";
                            trace += @" ·|LINE·|: " + st.GetFrame(a).GetFileLineNumber() + @".";
                            trace += "\r\n";
                        }
                    }
                    else
                        break;
                }
            }

            if (!string.IsNullOrEmpty(trace))
                trace = trace.Trim();

            return trace;
        }

        public string ValueInSplit_Get(string path, string separatorString1, string separatorString2, int position)
        {
            string valor = string.Empty;
            string[] pathSplit1 = path.Split(new string[] { separatorString1 }, StringSplitOptions.None);

            if (pathSplit1 != null && pathSplit1.Count() - 1 > 0)
            {
                for (int a = 0; a < pathSplit1.Count() - 1; a++)
                {
                    string[] pathSplit2 = pathSplit1[a].Split(new string[] { separatorString2 }, StringSplitOptions.None);

                    if (pathSplit2 != null && pathSplit2.Count() > position)
                    {
                        valor += pathSplit2[position] + separatorString1;
                    }
                }

                if (!string.IsNullOrEmpty(valor) && valor.Trim().Length > 0)
                    valor = valor.Remove(valor.Length - 1);
            }

            return valor;
        }
    }
}