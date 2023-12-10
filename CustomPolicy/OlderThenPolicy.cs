using Microsoft.AspNetCore.Authorization;

namespace AspMvcAuth.CustomPolicy
{
	public class OlderThenPolicy : IAuthorizationRequirement
	{
		public int Age { get; set; }
		public OlderThenPolicy(int age) 
		{ 
			Age = age;
		}
	}
}
