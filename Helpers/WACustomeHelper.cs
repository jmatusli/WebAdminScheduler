using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Oracle.ManagedDataAccess.Client;
using System.Collections;
using System.Linq.Dynamic.Core;
using WebAdminScheduler.Models;
using System.Linq; 

namespace WebAdminScheduler.helpers
{
    public  class WACustomHelper
   {
      public static int GetLasIdCRON(WebAdminSchedulerContext WAContext)
      {
         CP_CRONTAB data = (from s in WAContext.CP_CRONTABS.OrderByDescending(x => x.IDCRONTAB)
                           select s).ToList().AsQueryable().FirstOrDefault();
                  int LasIdcrontab=(data?.IDCRONTAB?? 0)+1;
                  return LasIdcrontab;
      }
      public static int GetLastIdPROC(WebAdminSchedulerContext WAContext)
      {
         CP_PROCESOS data = (from s in WAContext.CP_PROCESOS.OrderByDescending(x => x.IDPROC)
                           select s).ToList().AsQueryable().FirstOrDefault();
                  int LastIdproc=(data?.IDPROC?? 0)+1;
                  return LastIdproc;
      }

       public static int GetLastIDDEP(WebAdminSchedulerContext WAContext)
      {
         CP_DEPENDENCIAS data = (from s in WAContext.CP_DEPENDENCIAS.OrderByDescending(x => x.IDDEP)
                           select s).ToList().AsQueryable().FirstOrDefault();
                  int LastIddep= (data?.IDDEP ?? 0)+1;
                  return LastIddep;
      }

      public static  ArrayList getHijos(WebAdminSchedulerContext WAContext,int idproc)
      {
         ArrayList conections = new ArrayList(); 
         WAContext.Database.OpenConnection();
            String _query = "SELECT FP.parent_id FROM ("
            + " WITH dependenciatree(id, parent_id, path) AS (" 
            + " SELECT  idPROC,0 parent_id,TO_CHAR(idPROC) AS path FROM "
            + "        APP_SCL_ALTAMIRA.CP_PROCESOS cp WHERE CP.IDPROC NOT IN( "
            + "         SELECT IDPROC FROM APP_SCL_ALTAMIRA.CP_DEPENDENCIAS cd ) "
            + "  UNION ALL "
            + " SELECT   n2.idproc, n2.IDPROC_DEP parent_id,  dependenciatree.path || '-' || n2.IDPROC AS path "
            + " FROM APP_SCL_ALTAMIRA.CP_DEPENDENCIAS  n2,dependenciatree "
            + " WHERE n2.IDPROC_DEP  = dependenciatree.id) "
            + " SEARCH DEPTH FIRST BY id SET xorder "
            + " SELECT id,parent_id,path "
            + " FROM dependenciatree "
            + " order by xorder "
            + " )FP WHERE id=:pProc ";
 
            Console.WriteLine(" EN EJECUCION "+_query);
            OracleCommand oraCommand = new OracleCommand(_query,
            (OracleConnection)WAContext.Database.GetDbConnection());
            oraCommand.Parameters.Add(new OracleParameter("pProc", idproc));
            OracleDataReader oraReader = oraCommand.ExecuteReader();
            if (oraReader.HasRows)
            {
                while (oraReader.Read())
                {
                  Console.WriteLine("este es un hijo:"+oraReader.GetInt32(0));
                  conections.Add(oraReader.GetInt32(0));
                }
            }
         
            return  (conections.Count>0? conections:null);
            
      }
       public static  List<List<object>> GetAllProcs(WebAdminSchedulerContext WAContext,int idproc)
      {
         
        List<object> nodeDataList = new List<object>(); 
        List<int> listnodos = new List<int>();
 
    //    var conectionsFirst = new ArrayList(); //para almacenar conecciones segun nodo
  //       var conectionsFirst = new ArrayList(); //para almacenar conecciones segun nodo

 var conectionsFirst = new ArrayList(); 
 
           WAContext.Database.OpenConnection();
            String _query = "SELECT FP.*,length(FP.PATH) - length(replace(FP.PATH,'-')) nivel FROM ("
            + " WITH dependenciatree(id, parent_id, path) AS (" 
            + " SELECT  idPROC,0 parent_id,TO_CHAR(idPROC) AS path FROM "
            + "        APP_SCL_ALTAMIRA.CP_PROCESOS cp WHERE CP.IDPROC NOT IN( "
            + "         SELECT IDPROC FROM APP_SCL_ALTAMIRA.CP_DEPENDENCIAS cd ) "
            + "  UNION ALL "
            + " SELECT   n2.idproc, n2.IDPROC_DEP parent_id,  dependenciatree.path || '-' || n2.IDPROC AS path "
            + " FROM APP_SCL_ALTAMIRA.CP_DEPENDENCIAS  n2,dependenciatree "
            + " WHERE n2.IDPROC_DEP  = dependenciatree.id) "
            + " SEARCH DEPTH FIRST BY id SET xorder "
            + " SELECT id,parent_id,path "
            + " FROM dependenciatree "
            + " order by xorder "
            + " )FP WHERE id=:pProc ";
 
            Console.WriteLine(" EN EJECUCION "+_query);
            OracleCommand oraCommand = new OracleCommand(_query,
            (OracleConnection)WAContext.Database.GetDbConnection());
            oraCommand.Parameters.Add(new OracleParameter("pProc", idproc));
            OracleDataReader oraReader = oraCommand.ExecuteReader();
            var conteonodos=0;
            var conteoniveles=0;
              var conteoregistro=0;

            List<List<object>> niveles = new List<List<object>>();
            List<List<int>> directionstmp = new List<List<int>>();
            List<object> nodos = new List<object>(); 
            
            var conections = new ArrayList();
 

             if (oraReader.HasRows)
            {
                while (oraReader.Read())
                {

                  
                  Console.WriteLine("Elemento a procesar ID:"+oraReader.GetInt32(0)
                  +" PADRE: "+oraReader.GetInt32(1)
                  +" PATH: "+oraReader.GetString(2)
                  +" NIVEL: "+oraReader.GetInt32(3)
                  );
                var iddproc=oraReader.GetInt32(0);
                var parentid=oraReader.GetInt32(1);

                String[] path= oraReader.GetString(2).Split('-');
                String[] path2= oraReader.GetString(2).Split('-');
                var level=oraReader.GetInt32(3);
                path=  Enumerable.Reverse(path).ToArray();
                  Console.WriteLine("longitud de matriz path "+path2.Count());
                 
 

		               conteoniveles=0;
                   conteonodos=0;
		               foreach (string vpath in path) 
                       {
                                // if(Int32.Parse(vpath)!=idproc && Int32.Parse(vpath)!=parentid)
                                // nodeDataList.Add(new { id=parentid,connections=conections,level=level});
                                List<object> nodotmp = new List<object>(); 
                                List<object> nodotmp2 = new List<object>(); 
                                //nodos.Add(new { id=parentid,connections=conections,level=level});
                         
                             conections  =getHijos( WAContext,Int32.Parse(vpath));
    
                          if(conteoregistro==0)
                          {     
                            
                                 Console.WriteLine("no existe el nivel 0");
                                 if(Int32.Parse(vpath)==idproc)
                                 nodotmp.Add(new {  x=0,y=0,id=Int32.Parse(vpath),connections=(!conections.Contains(0)?conections:[]),level=conteoniveles});
                                 else 
                                  nodotmp.Add(new { id=Int32.Parse(vpath),connections=(!conections.Contains(0)?conections:[]),level=conteoniveles});
                             
                                niveles.Add(nodotmp);
                                listnodos.Add(Int32.Parse(vpath));
                                 conteonodos=conteonodos+1;
                          }   
                          else  
                          {
                             if(Int32.Parse(vpath)!=idproc ){
                           nodotmp.Add(new { id=Int32.Parse(vpath),connections=conections,level=conteoniveles});
                                   if(!listnodos.Contains(Int32.Parse(vpath)))
                                  {
                                  nodotmp2.Add(new { id=Int32.Parse(vpath),connections=(!conections.Contains(0)?conections:[]),level=conteoniveles});
                                 niveles.Add(nodotmp2);
                                 listnodos.Add(Int32.Parse(vpath));
                                 conteonodos=conteonodos+1;
                                 // conectionsFirst.Add(vpath);
                                  }
                                Console.WriteLine(" esto es el siguiente registro ");
                            }
                          }

                         conteoniveles=conteoniveles+1;
                      
                       }
              
                    
                   conteoregistro=conteoregistro+1; 
                }

                Console.WriteLine("mayor nivel encontrado"+(conteoniveles-1));
                  //var c=new {x=0,y=0,id=idproc,connections=conectionsFirst,level=0};
                  //  nodeDataList.Add(c);   
            
    
            }    
          
           return    niveles;
       
      }
   }
}