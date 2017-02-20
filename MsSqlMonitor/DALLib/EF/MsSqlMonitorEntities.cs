using DALLib.Models;
using Model = DALLib.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections;
using System.Data.Entity;
using Microsoft.AspNet.Identity;

namespace DALLib.EF
{
    public class MsSqlMonitorEntities : IdentityDbContext<User, Role, int, UserLogin, UserRole, UserClaim>
    {
        public MsSqlMonitorEntities()
            : base(GetConnectionString())
        {
            
        }
        public static MsSqlMonitorEntities Create()
        {
            return new MsSqlMonitorEntities();
        }
        public static String GetConnectionString()
        {
            log4net.Config.XmlConfigurator.Configure();

            Hashtable hash = new Hashtable();


            hash.Add("YURA-PC", @"Data Source=YURA-PC\SQLEXPRESS;Initial Catalog=db1;Integrated Security=False;MultipleActiveResultSets=True;Persist Security Info=True;User ID=sa;Password=awsaws");
            hash.Add("ANDRIY", @"Data Source=ANDRIY\SQLEXPRESS;Initial Catalog=db1;Integrated Security=True;MultipleActiveResultSets=True;");
            hash.Add("ACER", @"Data Source=ACER\SQLEXPRESS;Initial Catalog=db1;Integrated Security=True;MultipleActiveResultSets=True;");
            hash.Add("NOTIK-03", @"Data Source=NOTIK-03\MSSQL2012;Initial Catalog=db1;Integrated Security=False;MultipleActiveResultSets=True;User ID=sa;Password=qwerty12345");
            hash.Add("HP650", @"Data Source=HP650\SQLEXPRESS;Initial Catalog=db1;Integrated Security=True;MultipleActiveResultSets=True;");
            hash.Add("IVAN-ой", @"Data Source=IVAN-ой\SQLSERV2008EXPR;Initial Catalog=MsSqlMonitorEntities;Integrated Security=True;MultipleActiveResultSets=True;");
            hash.Add("TEST-PC", @"Data Source=TEST-PC\SQLEXPRESS;Initial Catalog=MsSqlMonitorEntities;Integrated Security=False;MultipleActiveResultSets=True;User ID=sa;Password=sa");

            String connString = (String)hash[Environment.MachineName];

            return connString;
        }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InstLogin>()
                .HasMany(il => il.Roles)
                .WithMany(ir => ir.Logins)                
                .Map(lr => lr.ToTable("InstRoleMembers").MapLeftKey("LoginId").MapRightKey("RoleId")); 

            modelBuilder.Entity<DbUser>()
                .HasMany(du => du.Roles)
                .WithMany(dr => dr.Users)
                .Map(ur => ur.ToTable("DbRoleMembers").MapLeftKey("UserId").MapRightKey("RoleId"));

            modelBuilder.Entity<Credential>()
                .HasRequired(c => c.User)
                .WithMany(u => u.Credentials)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<InstPrincipal>()
                .HasMany(ip => ip.Permissions)
                .WithMany(ip => ip.Principals)
                .Map(pp => pp.ToTable("InstPrincipalPermission").MapLeftKey("PrincipalId").MapRightKey("PermissionId"));

            modelBuilder.Entity<DbPrincipal>()
                .HasMany(dp => dp.Permissions)
                .WithMany(dp => dp.Principals)
                .Map(pp => pp.ToTable("DbPrincipalPermission").MapLeftKey("PrincipalId").MapRightKey("PermissionId"));
        }
        public virtual DbSet<Instance> Instances { get; set; }

        public virtual DbSet<Assign> Assigns { get; set; }
        public virtual DbSet<Model.Database> Databases { get; set; }
        public virtual DbSet<InstPrincipal> InstPrincipals { get; set; }
        public virtual DbSet<DbPrincipal> DbPrincipals { get; set; }
        public virtual DbSet<DbPermission> DbPermissions { get; set; }
        public virtual DbSet<InstPermission> InstPermissions { get; set; }
        public virtual DbSet<InstLogin> InstLogins { get; set; }
        public virtual DbSet<InstRole> InstRoles { get; set; }
        public virtual DbSet<DbUser> DbUsers { get; set; }
        public virtual DbSet<DbRole> DbRoles { get; set; }
        public virtual DbSet<InstanceVersion> InstanceVersions { get; set; }
        public virtual DbSet<JobType> JobTypes { get; set; }
    }
}