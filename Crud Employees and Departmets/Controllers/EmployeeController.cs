using Crud_Employees_and_Departmets.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Crud_Employees_and_Departmets.Controllers
{
    public class EmployeeController : Controller
    {
        CrudEmpDepartEntities db = new CrudEmpDepartEntities();
        // GET: Employee
        public ActionResult Index(int id)
        {
            List<Employee> employees = db.Employees.Where(a => a.Department_id == id).ToList();
            ViewBag.DepartmentId = id;
           
            return View(employees);
           
        }
       
        public JsonResult Add(Employee employee)
        {
            return Json(insert(employee), JsonRequestBehavior.AllowGet);
        }
        public int insert(Employee employee)
        {

            try
            {
                

                db.Employees.Add(employee);
                db.SaveChanges();
                return employee.Id;
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
            var Employee = db.Employees.Where(x => x.Id == ID).FirstOrDefault();
            return Json(Employee, JsonRequestBehavior.AllowGet);
        }
        public JsonResult Update(Employee employee)
        {
            return Json(UpdateEmployee(employee), JsonRequestBehavior.AllowGet);
        }
        public int UpdateEmployee(Employee employee)
        {
            Employee emp = db.Employees.Where(x => x.Id == employee.Id).FirstOrDefault();
            emp.Name = employee.Name;
            emp.EmpAddress = employee.EmpAddress;
            emp.phone = employee.phone;

            db.SaveChanges();

            return employee.Id;

        }
        public JsonResult Delete(int ID)
        {
            return Json(deleteEmp(ID), JsonRequestBehavior.AllowGet);
        }
        public int deleteEmp(int Id)
        {
            Employee emp = db.Employees.Where(x => x.Id == Id).FirstOrDefault();

            db.Employees.Remove(emp);
            db.SaveChanges();

            return emp.Id;

        }
    }
}