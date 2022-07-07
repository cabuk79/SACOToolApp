using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SolidWorks.Interop.sldworks;
using SldWorks;

namespace SolidWorksPressTooling.SwAPI
{
    public class Testing
    {
        SldWorks.SldWorks swApp = new SldWorks.SldWorks();

        public Testing()
        {
            //swApp.Visible = true;
        }

        public SldWorks.SldWorks ConnectToSW()
        {
            //var templatePath = @"C:\Users\cabuk\Desktop\SACO Metal and Insert.prtdot";
            swApp.Visible = true;
            //SldWorks.ModelDoc2 swdoc = swApp.NewDocument(templatePath, 0, 0, 0);

            return swApp;
        }
    }
}
