using System;
using System.Collections.Generic;
using System.Linq;
using doob.Reflectensions;
using Mapster;
using Who.Auth.Entities;
using Who.Auth.Entities.DTO;

namespace Who.Auth
{
    public class MapsterAdapterConfig
    {
        public MapsterAdapterConfig()
        {
            
        }

        public TypeAdapterConfig Build()
        {
            var config = new TypeAdapterConfig();

            

            config.NewConfig<User, UserListDto>()
                .Map(dest => dest.HasPassword, src => !String.IsNullOrWhiteSpace(src.Password));

            config.NewConfig<User, UserDto>()
                .Map(dest => dest.HasPassword, src => !String.IsNullOrWhiteSpace(src.Password));


            config.NewConfig<Role, RoleDto>();

            config.NewConfig<RoleDto, Role>();

            ClientAdapterConfig(config);

            return config;
        }


        private void ClientAdapterConfig(TypeAdapterConfig config)
        {

            config.NewConfig<CreateClientDto, Client>()
                .MapToTargetWith((src, dest) => MapPKCE(src, dest));

            config.NewConfig<Client, CreateClientDto>()
                .Map(dest => dest.RequirePkce, src => (src.Requirements ?? "").Contains("ft:pkce"));

            config.NewConfig<ClientDto, Client>()
                .AfterMappingInline((src, dest) => MapPKCE(src, dest));
                //.MapToTargetWith((src, dest) => MapPKCE(src, dest));

            config.NewConfig<Client, ClientDto>()
                
                .Map(dest => dest.RequirePkce, src => (src.Requirements ?? "").Contains("ft:pkce"));

           

            config.NewConfig<ClientRedirectUri, ClientRedirectUriDto>();
            config.NewConfig<ClientRedirectUriDto, ClientRedirectUri>();
        }

        private Client MapPKCE(CreateClientDto src, Client dest)
        {
            var ls = dest.Requirements?.StartsWith("[") == true
                ? Json.Converter.ToObject<List<string>>(dest.Requirements)
                : new List<string>();

                    
            if (src.RequirePkce)
            {
                if (!ls.Contains("ft:pkce"))
                {
                    ls.Add("ft:pkce");
                }
            }
            else
            {
                if (ls.Contains("ft:pkce"))
                {
                    ls.Remove("ft:pkce");
                }
            }

            dest.Requirements = Json.Converter.ToJson(ls);
            return dest;
        }

        private string MapPKCE1(CreateClientDto src, Client dest)
        {
            var ls = dest.Requirements.Split(";").ToList();

            if (src.RequirePkce)
            {
                if (!ls.Contains("ft:pkce"))
                {
                    ls.Add("ft:pkce");
                }
            }
            else
            {
                if (ls.Contains("ft:pkce"))
                {
                    ls.Remove("ft:pkce");
                }
            }

            return String.Join(";", ls);
        }

        private Client MapPKCE(ClientDto src, Client dest)
        {
            var ls = dest.Requirements?.StartsWith("[") == true
                ? Json.Converter.ToObject<List<string>>(dest.Requirements)
                : new List<string>();

                    
            if (src.RequirePkce)
            {
                if (!ls.Contains("ft:pkce"))
                {
                    ls.Add("ft:pkce");
                }
            }
            else
            {
                if (ls.Contains("ft:pkce"))
                {
                    ls.Remove("ft:pkce");
                }
            }

            dest.Requirements = Json.Converter.ToJson(ls);
            return dest;
        }
    }
}
