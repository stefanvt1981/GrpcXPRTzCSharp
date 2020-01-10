using GrpcXPRTzCSharp.Repository.Entities;
using Messages;
using Messages.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GrpcXPRTzCSharp.Repository.Mappers
{
    public static class XprtMapper
    {
        public static Xprt ToDTO(this XprtEntity entity)
        {
            var xprt = new Xprt 
            { 
                BadgeNumber = entity.BadgeNumber,
                FirstName = entity.FirstName,
                GeboorteDatum = Utils.ToUnixTimestamp(entity.GeboorteDatum),
                Id = entity.Id,
                LastName = entity.LastName
            };

            var skills = entity.Skills.Split(',').ToList();
            skills.ForEach(s => xprt.Skills.Add(s));

            return xprt;
        }

        public static XprtEntity ToEntity(this Xprt dto)
        {
            var entity = new XprtEntity
            {
                BadgeNumber = dto.BadgeNumber,
                FirstName = dto.FirstName,
                GeboorteDatum = Utils.FromUnixTimestamp(dto.GeboorteDatum),
                Id = dto.Id,
                LastName = dto.LastName                
            };

            entity.Skills = string.Join(',', dto.Skills);

            return entity;
        }
    }
}
