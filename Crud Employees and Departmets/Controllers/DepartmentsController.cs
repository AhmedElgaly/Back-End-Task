using Crud_Employees_and_Departmets.Models;
using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crud_Employees_and_Departmets.Controllers
{
    public class DepartmentsController : Controller
    {
        CrudEmpDepartEntities db = new CrudEmpDepartEntities();
        // GET: Departments
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult List()
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Department> departments = db.Departments.ToList();
            return Json(departments, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Add(Department department)
        {
            return Json(insert(department), JsonRequestBehavior.AllowGet);
        }
        public int insert(Department department)
        {
           
            try
            {
                db.Departments.Add(department);
                db.SaveChanges();
                return department.Id;
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            
        }
        public JsonResult GetbyID(int ID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var Department = db.Departments.Where(x => x.Id == ID).FirstOrDefault();
            return Json(Department, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Department department)
        {
            return Json(UpdateDepartment(department), JsonRequestBehavior.AllowGet);
        }
        public int UpdateDepartment(Department department)
        {
            Department dept = db.Departments.Where(x => x.Id == department.Id).FirstOrDefault();
            dept.Name = department.Name;
            
            db.SaveChanges();

            return department.Id;

        }
        public JsonResult Delete(int ID)
        {
            return Json(deleteCat(ID), JsonRequestBehavior.AllowGet);
        }
        public int deleteCat(int Id)
        {
            Department dept = db.Departments.Where(x => x.Id == Id).FirstOrDefault();

            db.Departments.Remove(dept);
            db.SaveChanges();

            return dept.Id;

        }
       
        public ActionResult ExportEmployees()
        {
            List<Employee> allEmployees = new List<Employee>();
            allEmployees = db.Employees.ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "DepartmentReprot.rpt"));

            rd.SetDataSource(allEmployees);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "EmployeeList.pdf");
        }

        public ActionResult ExportDepartments()
        {
            List<Department> allDepartments = new List<Department>();
            allDepartments = db.Departments.ToList();


            ReportDocument rd = new ReportDocument();
            rd.Load(Path.Combine(Server.MapPath("~/Reports"), "DeptReport.rpt"));

            rd.SetDataSource(allDepartments);

            Response.Buffer = false;
            Response.ClearContent();
            Response.ClearHeaders();


            Stream stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            stream.Seek(0, SeekOrigin.Begin);
            return File(stream, "application/pdf", "DepartmentsList.pdf");
        }

    }
}