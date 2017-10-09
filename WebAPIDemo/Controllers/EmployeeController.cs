using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using EmployeeDataAccess;

namespace WebAPIDemo.Controllers
{
    public class EmployeeController : ApiController
    {
        [HttpGet]
        public HttpResponseMessage Get(string Gender = "All")
        {
            try
            {
                using (EmployeeDBEntities employeeDBRntities = new EmployeeDBEntities())
                {
                    switch (Gender.ToLower())
                    {
                        case "all":
                            return Request.CreateResponse(HttpStatusCode.OK, employeeDBRntities.Employees.ToList());
                        case "male":
                            return Request.CreateResponse(HttpStatusCode.OK, employeeDBRntities.Employees.Where(e => e.Gender.ToLower() == "male").ToList());
                        case "female":
                            return Request.CreateResponse(HttpStatusCode.OK, employeeDBRntities.Employees.Where(e => e.Gender.ToLower() == "female").ToList());
                        default:
                            return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Please enter male , female or all ");
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpGet]
        public HttpResponseMessage Get(int Id)
        {
            try
            {
                using (EmployeeDBEntities employeeDBRntities = new EmployeeDBEntities())
                {
                    var entity = employeeDBRntities.Employees.FirstOrDefault(e => e.ID == Id);
                    if (entity != null)
                    {
                        var message = Request.CreateResponse(HttpStatusCode.OK, entity);
                        message.Headers.Location = new Uri(Request.RequestUri.ToString());
                        return message;
                    }
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Employee not found with id: {0}" + Id.ToString());
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [HttpPost]
        public HttpResponseMessage Post([FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities employeeDBRntities = new EmployeeDBEntities())
                {
                    employeeDBRntities.Employees.Add(employee);
                    employeeDBRntities.SaveChanges();
                }
                var message = Request.CreateResponse(HttpStatusCode.Created, employee);
                message.Headers.Location = new Uri(Request.RequestUri + employee.ID.ToString());
                return message;
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);

            }

        }

        [HttpDelete]
        public HttpResponseMessage Delete(int Id)
        {
            try
            {
                using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    var entity = employeeDBEntities.Employees.FirstOrDefault(e => e.ID == Id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "not exist");

                    }
                    else
                    {
                        employeeDBEntities.Employees.Remove(entity);
                        employeeDBEntities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, entity);
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }

        [HttpPut]
        public HttpResponseMessage Put(int Id, [FromBody]Employee employee)
        {
            try
            {
                using (EmployeeDBEntities employeeDBEntities = new EmployeeDBEntities())
                {
                    var entity = employeeDBEntities.Employees.FirstOrDefault(e => e.ID == Id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "not exist");

                    }
                    else
                    {
                        entity.FirstName = employee.FirstName;
                        entity.LastName = employee.LastName;
                        entity.Gender = employee.Gender;
                        entity.Salary = employee.Salary;
                        employeeDBEntities.SaveChanges();
                        var message = Request.CreateResponse(HttpStatusCode.OK, entity);
                        return message;
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex);
            }
        }
    }

}
