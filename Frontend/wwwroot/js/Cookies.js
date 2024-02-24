 export const AddCookies = (name, value, daysToExpire) => {
    let date = new Date();
    date.setTime(date.getTime() + (daysToExpire * 24 * 60 * 60 * 1000));
    let expires = `expires=${date.toUTCString()}`
     document.cookie = `${name}=${value};${expires};path=/`;

     return document.cookie.includes(`${name}=${value}`);
}

 export const GetCookie = (name) => {
    let cookieName = name;
    let decodeCookie = decodeURIComponent(document.cookie);
    let cookiesArray = decodeCookie.split(";");
    for (let i = 0; i < cookiesArray.length; i++) {
        let cookie = cookiesArray[i];
        while (cookie.charAt(0) == ' ') {
            cookie = cookie.substring(1);
        }
        if (cookie.indexOf(cookieName) == 0) {
            return cookie.substring(cookie.indexOf('=') + 1);
        }
    }
    return "";
}

export const DeleteCookie = (name) => {
    document.cookie = `${name}=; expires=Thu, 01 Jan 1970 00:00:00 UTC; path=/;`;
}