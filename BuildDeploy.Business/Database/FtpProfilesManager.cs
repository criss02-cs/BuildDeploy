using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BuildDeploy.Business.Entity;
using Microsoft.EntityFrameworkCore;

namespace BuildDeploy.Business.Database;
public class FtpProfilesManager
{
    public async Task<List<FtpProfile>> GetAllProfiles()
    {
        return await DatabaseContext.Instance.FtpProfiles.ToListAsync();
    }

    public async Task<bool> AddOrUpdateProfile(FtpProfile ftpProfile)
    {
        var existingProfile = await DatabaseContext.Instance.FtpProfiles.AnyAsync(p => p.Id == ftpProfile.Id);
        if (!existingProfile)
        {
            return await AddProfile(ftpProfile);
        }
        return await UpdateProfile(ftpProfile);
    }

    private async Task<bool> UpdateProfile(FtpProfile ftpProfile)
    {
        DatabaseContext.Instance.FtpProfiles.Update(ftpProfile);
        var result = await DatabaseContext.Instance.SaveChangesAsync();
        return result == 1;
    }

    private async Task<bool> AddProfile(FtpProfile ftpProfile)
    {
        DatabaseContext.Instance.FtpProfiles.Add(ftpProfile);
        var result = await DatabaseContext.Instance.SaveChangesAsync();
        return result == 1;
    }
}
