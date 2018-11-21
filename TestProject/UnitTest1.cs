using Host.Business.IDbServices;
using System;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        private readonly IEmployeesService _employeesService;

        public UnitTest1(IEmployeesService employeesService)
        {
            _employeesService = employeesService;
        }

        [Fact]
        public void Test1()
        {
            var result = _employeesService.GetAllEmployee();
        }
    }
}
