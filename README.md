# MinimalApiLabb3
Version of Lab3 but with minimalAPI as basic as I think it could be.
# Om uppgiften

I den här labben ska du testa att bygga ditt första enkla Webb-API. Det API du kommer konstruera använder en REST-arkitektur och kommer möjliggöra för externa tjänster och applikationer att hämta och ändra data i din egen applikation.

# Vad du ska göra

<aside>
⚙️ **Applikationen/databasen**

Det första du ska skapa är en väldigt grundläggande applikation med en databas som klarar följande.

- [ ]  Det ska gå att lagra personer med grundläggande information om dem som namn och telefonnummer.
- [ ]  Systemet ska kunna lagra ett obegränsat antal intressen som de har. Varje intresse ska ha en titel och en kort beskrivning.
- [ ]  Varje person ska kunna vara kopplad till ett valfritt antal intressen
- [ ]  Det ska gå att lagra ett obegränsat antal länkar (till webbplatser) till varje intresse för varje person. Om en person lägger in en länk så är den alltså kopplad både till den personen och till det intresset.
</aside>

<aside>
🗣 **Skapa ett REST-API**

Det andra steget du ska göra är att skapa ett REST-API som tillåter externa tjänster att utföra följande anrop till ditt API samt genomför dessa förändringar i din applikation.

- [ ]  Hämta alla personer i systemet
- [ ]  Hämta alla intressen som är kopplade till en specifik person
- [ ]  Hämta alla länkar som är kopplade till en specifik person
- [ ]  Koppla en person till ett nytt intresse
- [ ]  Lägga in nya länkar för en specifik person och ett specifikt intresse

---

**Extra utmaning (gör om du vill)**

- [ ]  Ge möjlighet till den som anropar APIet och efterfrågar en person att direkt få ut alla intressen och alla länkar för den personen i en hierarkisk JSON-fil
- [ ]  Ge möjlighet för den som anropar APIet att filtrera vad den får ut, som en sökning. Exempelvis som jag skickar med “to” till hämtning av alla personer vill jag ha de som har ett “to” i namnet så som “tobias” eller “tomas”. Detta kan du sen skapa för alla anropen om du vill.
- [ ]  Skapa paginering av anropen. När jag anropar exempelvis personer får jag kanske de första 100 personerna och får sen anropa ytterligare gånger för att få fler. Här kan det också vara snyggt att anropet avgör hur många personer jag får i ett anrop så jag kan välja att få säg 10st om jag bara vill ha det.
</aside>

<aside>
🕹️ **Testa ditt API**

Det sista steget är att göra anrop mot ditt API genom tjänsten [Postman](https://www.postman.com/) eller swagger. 

- [ ]  Gör ett anrop för varje krav ovan för API:et
- [ ]  I din readme-fil i Git lägger du in alla anrop du gjort för varje krav ovan kring APIet så vi kan se hur du tänker dig att anropen ska se ut.
</aside>
