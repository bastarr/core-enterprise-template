using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Acme.WebCore.MappingProfiles
{
    public class BusinessUnitMappingProfile : Profile
    {
        public BusinessUnitMappingProfile()
        {
            #region Domain to Model
            CreateMap<Core.Domain.BusinessUnit, Models.BusinessUnitModel>();
            #endregion

            #region Model to Domain
            CreateMap<Models.BusinessUnitModel, Core.Domain.BusinessUnit>();
            #endregion
        }
    }
}
