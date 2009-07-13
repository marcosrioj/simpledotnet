using System;
using Simple.Services;
using Simple.Filters;
using FluentNHibernate.Mapping;
using Simple.ConfigSource;

namespace Simple.Tests.Contracts
{

    [Serializable, DefaultConfig(typeof(DBEnsurer))]
    public partial class EmpresaFuncionario : Entity<EmpresaFuncionario, IEmpresaFuncionarioRules>
    {
        public virtual int Id { get; set; }
        public static PropertyName IdProperty = "Id";

        public virtual Funcionario Funcionario { get; set; }
        public static PropertyName FuncionarioProperty = "Funcionario";

        public virtual Empresa Empresa { get; set; }
        public static PropertyName EmpresaProperty = "Empresa";

        public class Map : ClassMap<EmpresaFuncionario>
        {
            public Map()
            {
                Not.LazyLoad();

                Id(e => e.Id).GeneratedBy.Identity();
                HasOne(e => e.Funcionario).SetAttribute("lazy", "false");
                HasOne(e => e.Empresa).SetAttribute("lazy", "false");
            }
        }

    }
}