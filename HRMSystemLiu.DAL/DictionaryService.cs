using HRMSystemLiu.Model;
using HRMSystemLiu.Model.Database;

namespace HRMSystemLiu.DAL;

public static class DictionaryService
{
    private static async Task<List<Dictionary>> GetDictionaryAsync(string category)
    {
        var dict = await SqlHelper.QueryStringGuidDictionaryAsync(
            $"SELECT Id, Name FROM Dictionary WHERE Category = '{category}'",
            "Name",
            "Id");
        return dict.Select(pair => new Dictionary { Name = pair.Key, Id = pair.Value }).ToList();
    }

    public static async Task<List<Dictionary>> GetMarriageIdDictionaryAsync()
    {
        return await GetDictionaryAsync("婚姻状况");
    }

    public static async Task<List<Dictionary>> GetPartyIdDictionaryAsync()
    {
        return await GetDictionaryAsync("政治面貌");
    }

    public static async Task<List<Dictionary>> GetEducationIdDictionaryAsync()
    {
        return await GetDictionaryAsync("学历");
    }

    public static async Task<List<Dictionary>> GetGenderIdDictionaryAsync()
    {
        return await GetDictionaryAsync("性别");
    }

    public static async Task<DictionaryList> GetDictionarySheetAsync()
    {
        return new DictionaryList
        {
            MarriageIdDictionary = await GetMarriageIdDictionaryAsync(),
            GenderIdDictionary = await GetGenderIdDictionaryAsync(),
            EducationIdDictionary = await GetEducationIdDictionaryAsync(),
            PartyIdDictionary = await GetPartyIdDictionaryAsync()
        };
    }
}