﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace onlygodknows.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class LogisticsSoftEntities : DbContext
    {
        public LogisticsSoftEntities()
            : base("name=LogisticsSoftEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
        public virtual DbSet<approval> approvals { get; set; }
        public virtual DbSet<asignproject> asignprojects { get; set; }
        public virtual DbSet<AspNetRole> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaim> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogin> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUser> AspNetUsers { get; set; }
        public virtual DbSet<Attendance> Attendances { get; set; }
        public virtual DbSet<AttendanceFreq> AttendanceFreqs { get; set; }
        public virtual DbSet<Camp_Details> Camp_Details { get; set; }
        public virtual DbSet<CsPermission> CsPermissions { get; set; }
        public virtual DbSet<Holiday> Holidays { get; set; }
        public virtual DbSet<LabourMaster> LabourMasters { get; set; }
        public virtual DbSet<MainTimeSheet> MainTimeSheets { get; set; }
        public virtual DbSet<Manpowerinoutform> Manpowerinoutforms { get; set; }
        public virtual DbSet<ManPowerSupplier> ManPowerSuppliers { get; set; }
        public virtual DbSet<overtimeemployeelist> overtimeemployeelists { get; set; }
        public virtual DbSet<overtimeref> overtimerefs { get; set; }
        public virtual DbSet<ProjectList> ProjectLists { get; set; }
        public virtual DbSet<promanlist> promanlists { get; set; }
        public virtual DbSet<towemp> towemps { get; set; }
        public virtual DbSet<towref> towrefs { get; set; }
        public virtual DbSet<trattfile> trattfiles { get; set; }
        public virtual DbSet<username> usernames { get; set; }
        public virtual DbSet<ControlRoom> ControlRooms { get; set; }
        public virtual DbSet<Copy_of_Attendance> Copy_of_Attendances { get; set; }
        public virtual DbSet<EmpPromise> EmpPromises { get; set; }
        public virtual DbSet<LogIn> LogIns { get; set; }
        public virtual DbSet<MainShifting> MainShiftings { get; set; }
        public virtual DbSet<Mnth> Mnths { get; set; }
        public virtual DbSet<MyCompanyInfo> MyCompanyInfoes { get; set; }
        public virtual DbSet<PPP> PPPs { get; set; }
        public virtual DbSet<Pprint> Pprints { get; set; }
        public virtual DbSet<RefsList> RefsLists { get; set; }
        public virtual DbSet<Room_Details> Room_Details { get; set; }
        public virtual DbSet<ShiftingFile> ShiftingFiles { get; set; }
        public virtual DbSet<Switchboard_Item> Switchboard_Items { get; set; }
        public virtual DbSet<tblUser> tblUsers { get; set; }
        public virtual DbSet<USER_ROLES> USER_ROLES { get; set; }
        public virtual DbSet<UserInfo> UserInfoes { get; set; }
        public virtual DbSet<weekendlist> weekendlists { get; set; }
        public virtual DbSet<sharepointatt> sharepointatts { get; set; }
        public virtual DbSet<access_date> access_date { get; set; }
    }
}
