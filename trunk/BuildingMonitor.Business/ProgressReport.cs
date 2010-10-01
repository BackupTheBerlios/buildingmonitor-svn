using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using BuildingMonitor.Data;

namespace BuildingMonitor.Business
{
    public class ProgressReport
    {
		public static DataSet AvancePorcentual(int IdProyecto, int IdBloque, int IdObra, int IdGrupo)
		{
			return DBProgressReport.AvancePorcentual(IdProyecto, IdBloque, IdObra, IdGrupo);
		}

		public static DataSet AvancePorcentual_Bloque(int IdProyecto, int IdGrupo)
		{
			return DBProgressReport.AvancePorcentual_Bloque(IdProyecto, IdGrupo);
		}
    }
}
