namespace UnityRepository.Initializers
{
  using System.Collections.Generic;
  using System.Linq;
  using System.Data.Entity;
  using Entities;
  using Microsoft.AspNet.Identity.EntityFramework;
  using UnityRepository.Contexts;

  public class UnityDbInitializer : CreateDatabaseIfNotExists<UnityDbContext>
  {
    protected override void Seed(UnityDbContext context)
    {
     //context.Domicile.AddRange(this.GetDomicileSeed());
     //context.Mancos.AddRange(this.GetManCoSeed());
     //this.GetRolesSeed().ForEach(x => context.Roles.Add(x));
     //context.SecurityQuestions.AddRange(this.GetSecurityQuestionsSeed());
     //context.GlobalSettings.Add(this.GetGlobalSetting());

     //base.Seed(context);
    }

    private List<Domicile> GetDomicileSeed()
    {
      var listDomiciles = new List<Domicile>();

      listDomiciles.Add(new Domicile { Code = "Onshore", Description = "Onshore Management Companies Only" });
      listDomiciles.Add(new Domicile { Code = "DUB", Description = "Dublin" });
      listDomiciles.Add(new Domicile { Code = "GUE", Description = "Guernsey" });
      listDomiciles.Add(new Domicile { Code = "Lux", Description = "Luxembourg" });

      return listDomiciles;
    }

    private List<ManCo> GetManCoSeed()
    {
      var listManCos = new List<ManCo>();

      //Onshore ManCos
      listManCos.Add(new ManCo { Code = "014", Description = "Premier (14)", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "0003", Description = "Ignis Asset Management Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "0004", Description = "TU Fund Managers Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "0005", Description = "Cazenove investment Fund Management Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "0006", Description = "Taube Hodson Stonex Partners", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "0007", Description = "Hargreaves Lansdown Fund Management", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "0008", Description = "Ecclesiastical Investmnet Management Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "0009", Description = "NT demo company", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00010", Description = "Cavendish Asset Management Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00012", Description = "Dimensional Fund Advisours Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00015", Description = "FF&P Asset Management Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00016", Description = "Rothschild Private Fund Management Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00017", Description = "Premier Portfolio Managers Limited (17)", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00018", Description = "Sarasin Investment Funds Ltd", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00019", Description = "Baring Fund Managers Ltd", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00020", Description = "Melchior Investment Funds", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00021", Description = "Fulcrum Asset Management", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00022", Description = "Thesis Unit Trst Management Limited", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00023", Description = "Canaccord Genuity Limitied", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00024", Description = "Barclays Asset Management", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00025", Description = "Cordea Savills Investment Management", DomicileId = 1 });
      listManCos.Add(new ManCo { Code = "00026", Description = "Schroders Unit Trust Limited", DomicileId = 1 });

      //Dublin Mancos
      listManCos.Add(new ManCo { Code = "199625", Description = "ALBEMARLE", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "264", Description = "APS GROWTH FUND", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200023", Description = "African Alliance", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200165", Description = "Analytic Investors", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199771", Description = "Anima Prima Funds", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199740", Description = "Archipel Funds", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199214", Description = "Ashmore Cayman Domiciled Funds", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200389", Description = "Ashmore EEM", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199215", Description = "Ashmore Guernsey Domiciled Funds", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200448", Description = "Atlantis", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199624", Description = "BLOXHAM", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199769", Description = "Bank of Ireland PB", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199772", Description = "Barclays Wealth", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199932", Description = "Barclays Wealth IMPS", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "43", Description = "Bedlam Funds PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199338", Description = "Bosera", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200450", Description = "Controlfida", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199166", Description = "Coupland Cardiff Funds PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199744", Description = "Covestone AM", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199188", Description = "DAVY Inter", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199446", Description = "DELA", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199604", Description = "DEUT ASSET MANAG", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "100559", Description = "Diversified Credit Investments, LLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "283", Description = "EDM Asset Management Limited", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200026", Description = "EQI Irsih Funds", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199768", Description = "Edinburgh Partners", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199794", Description = "FTC Satori Fund", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199239", Description = "First Arrow Investment Management Funds", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199315", Description = "First Quadrant Funds PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199464", Description = "FundLogic Alternative", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199413", Description = "Gard Common Contractual Fund", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "250", Description = "Generation", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199416", Description = "HP Invest Common Contractual Fund", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199603", Description = "HUNTER HALL PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199247", Description = "Harding Loevner", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199685", Description = "Herald InvManLtd", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199449", Description = "Hermes BPK F PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199448", Description = "Hermes Investment Fund", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "100205", Description = "Hermes Investment Management Ltd", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199373", Description = "IBM", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199430", Description = "IFP", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199478", Description = "INGENIOUS FUNDS PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199503", Description = "IPM_CCF", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "282", Description = "Insight", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200090", Description = "Irish Pension Unit Trust", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200042", Description = "Iveagh Global Strategies", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199778", Description = "KBI", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199510", Description = "LGIM", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199289", Description = "Legal & General", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "100059", Description = "MARRIOTT INTERNATIONAL", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200453", Description = "MIO Compass", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199433", Description = "MONDRIAN", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199419", Description = "Mediolanum International Funds Limited", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200398", Description = "Morant Wright Management Ltd", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199786", Description = "NBAD One Share PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199810", Description = "NTGI Limited", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199811", Description = "Northern Trust Investment Funds PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199186", Description = "Northern Trust UCITS CCF", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "100004", Description = "OLDFIELD PARTNERS", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200468", Description = "Oaktree Capital Management", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199653", Description = "Ocean Dial", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "100013", Description = "PROGRESSIVE DEVELOPING MARKETS", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199596", Description = "QS Investors LLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "40", Description = "ROBUSTA", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "68", Description = "Record Currency Management", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200447", Description = "Riverwood", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "100562", Description = "SARASIN", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200022", Description = "SMEDVIG QIF PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199455", Description = "SPRING LAKE LIMITED", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200399", Description = "Schroders Ireland", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199339", Description = "Stenham", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199377", Description = "Stenham Guernsey", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199856", Description = "Stenham Investment Fund Cayman", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199530", Description = "Stenham Real Estate Equity Fund", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200349", Description = "Stone Drum Funds", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200442", Description = "Sustainable Insight", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200021", Description = "TT International Funds PLC", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200024", Description = "Tailwind", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200362", Description = "Thomas White International", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "199689", Description = "UOB Global Strat", DomicileId = 2 });
      listManCos.Add(new ManCo { Code = "200025", Description = "Warren Capital", DomicileId = 2 });

      //Guernsey Mancos
      listManCos.Add(new ManCo { Code = "199535", Description = "Momentum Mutual Fund ICC Limited", DomicileId = 3 });
      listManCos.Add(new ManCo { Code = "200496", Description = "Partners Group Ltd", DomicileId = 3 });
      listManCos.Add(new ManCo { Code = "100146", Description = "RUFFER", DomicileId = 3 });

      //Luxembourg Mancos
      listManCos.Add(new ManCo { Code = "199229", Description = "Ashmore Investment Management Limited", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "200104", Description = "Astellon Fund", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "199447", Description = "Baring Asset Management Limited", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "200171", Description = "Barclays Alternative", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "199493", Description = "CHILTON UCITS", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "200193", Description = "Duxton AM", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "199602", Description = "GOLDMAN SACHS", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "200474", Description = "IPM Setup", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "200132", Description = "R Wealth Management", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "200154", Description = "Schlumberger", DomicileId = 4 });
      listManCos.Add(new ManCo { Code = "200389", Description = "Ashmore EMM", DomicileId = 4 });
     
      return listManCos;
    }

    private List<IdentityRole> GetRolesSeed()
    {
      var listRoles = new List<IdentityRole>();

      listRoles.Add(new IdentityRole("DST-Admin"));
      listRoles.Add(new IdentityRole("Admin"));
      listRoles.Add(new IdentityRole("Governor"));
      listRoles.Add(new IdentityRole("Read Only"));

      return listRoles;
    }

    private List<SecurityQuestion> GetSecurityQuestionsSeed()
    {
      var listSecurityQuestions = new List<SecurityQuestion>();

      listSecurityQuestions.Add(new SecurityQuestion("Mother's Maiden Name"));
      listSecurityQuestions.Add(new SecurityQuestion("Name of First Pet"));
      listSecurityQuestions.Add(new SecurityQuestion("Memorable Date"));
      listSecurityQuestions.Add(new SecurityQuestion("Memorable Place"));
      listSecurityQuestions.Add(new SecurityQuestion("City of Birth"));
      listSecurityQuestions.Add(new SecurityQuestion("Memorable Name"));
      listSecurityQuestions.Add(new SecurityQuestion("Name of your First School"));
      listSecurityQuestions.Add(new SecurityQuestion("Date of Birth"));
      listSecurityQuestions.Add(new SecurityQuestion("Name of your Hometown"));
      listSecurityQuestions.Add(new SecurityQuestion("Name of Favourite Team"));

      return listSecurityQuestions;
    }

    private GlobalSetting GetGlobalSetting()
    {
      var globalSetting = new GlobalSetting();

      globalSetting.MinNonAlphaChars = 0;
      globalSetting.PasswordExpDays = 30;
      globalSetting.PassChangeBefore = 12;
      globalSetting.NewUserPasswordReset = false;
      globalSetting.MinPasswordLength = 5;

      return globalSetting;
    }
  }
}
