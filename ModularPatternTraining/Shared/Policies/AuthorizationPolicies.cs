using Microsoft.AspNetCore.Authorization;

namespace ModularPatternTraining.Shared.Policies
{
    public static class AuthorizationPolicies
    {
        public static void AddCustomPolicies(AuthorizationOptions options)
        {
          
            options.AddPolicy("SuperUserOnly", policy =>
                policy.RequireRole("61db5db3-912a-4c52-aa4a-ec1e9bddb6d1"));

           
            options.AddPolicy("AdminOrSuperUser", policy =>
                policy.RequireRole("14f925f8-181d-4486-870d-d8f5ec85c639", "61db5db3-912a-4c52-aa4a-ec1e9bddb6d1"));

           
            options.AddPolicy("MinimumAge", policy =>
                policy.RequireClaim("Age", "18"));
        }
    }
}
