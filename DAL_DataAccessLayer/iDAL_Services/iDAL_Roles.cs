﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DAL_DataAccessLayer.Entities;

namespace DAL_DataAccessLayer.iDAL_Services
{
    public interface iDAL_Roles
    {
        string AddRole(Roles role);
        string UpdateRole(Roles role);
        string RemoveRole(string id);
        Roles GetRoleById(string id);
        List<Roles> GetRoles();
    }
}
