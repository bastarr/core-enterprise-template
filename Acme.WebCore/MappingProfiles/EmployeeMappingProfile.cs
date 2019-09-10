using System;
using System.Collections.Generic;
using AutoMapper;

namespace Acme.WebCore.MappingProfiles
{
    public class EmployeeMappingProfile : Profile
    {
        public EmployeeMappingProfile()
        {
            #region Domain to Model
            CreateMap<Core.Domain.Employee, Models.EmployeeModel>();
            #endregion

            #region Model to Domain
            CreateMap<Models.EmployeeModel, Core.Domain.Employee>();
            #endregion
        }
    }
}
