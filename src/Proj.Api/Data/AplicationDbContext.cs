﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Proj.Api.Data
{
    // Criamos um novo contexto de acesso à dados por causa da herança de 'IdentityDbContext', mas isso
    //ñão exigirá a criação de um novo BD no sistema, pois a connections string é a mesma nesse caso.
    public class AplicationDbContext : IdentityDbContext
    {
        public AplicationDbContext(DbContextOptions<AplicationDbContext> options) : base(options) { }

    }
}
