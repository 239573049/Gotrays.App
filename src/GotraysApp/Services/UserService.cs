﻿namespace GotraysApp.Services;

public class UserService : CallerBase
{
    public UserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
    {
    }

    public async Task<string> Login(LoginPayloadDto dto)
    {
        var value = await PostStringAsync("v1/Users/Login", dto);

        return value;
    }

    public async Task<DayDosageDto> GetDayDosage()
    {
        return await GetAsync<DayDosageDto>("v1/Users/DayDosage");
    }

    public async Task<GetUserDto> GetGetUser()
    {
        return await GetAsync<GetUserDto>("v1/Users");
    }

}
