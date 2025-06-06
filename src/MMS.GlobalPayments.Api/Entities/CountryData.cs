﻿using System.Collections.Generic;

namespace MMS.GlobalPayments.Api.Entities {
    /// <summary>
    ///  Represents Country Data Dictionaries for Country Name, Alpha2, Alpha3 and Numeric.
    /// </summary>
    public class CountryData {

        private Dictionary<string, string> _countryCodeAlpha2MapByCountry;
        private Dictionary<string, string> _countryMapByAlpha2CountryCode;
        private Dictionary<string, string> _countryCodeAlpha3MapAlpha2CountryCode;
        private Dictionary<string, string> _countryCodeAlpha2MapAlpha3CountryCode;

        private Dictionary<string, string> _countryCodeAlpha3MapByCountry;
        private Dictionary<string, string> _countryMapByAlpha3CountryCode;
        private Dictionary<string, string> _countryCodeAlpha3MapByNumeric;
        private Dictionary<string, string> _numericCodeMapByAlpha3CountryCode;

        private Dictionary<string, string> _countryCodeAlpha2MapByNumericCode;
        private Dictionary<string, string> _numericCodeMapByCountryCodeAlpha2;
        private Dictionary<string, string> _numericCodeMapByCountry;
        private Dictionary<string, string> _countryMapByNumeric;

        /// <summary>
        /// Dictionaries Init 
        /// </summary>
        public CountryData(){
            #region Alpha2 Data Init

            #region Alpha2byCountryName - CountryNameByAlpha2
            _countryCodeAlpha2MapByCountry = new Dictionary<string, string>();
            _countryCodeAlpha2MapByCountry.Add("Afghanistan", "AF");
            _countryCodeAlpha2MapByCountry.Add("Åland Islands", "AX");
            _countryCodeAlpha2MapByCountry.Add("Albania", "AL");
            _countryCodeAlpha2MapByCountry.Add("Algeria", "DZ");
            _countryCodeAlpha2MapByCountry.Add("American Samoa", "AS");
            _countryCodeAlpha2MapByCountry.Add("Andorra", "AD");
            _countryCodeAlpha2MapByCountry.Add("Angola", "AO");
            _countryCodeAlpha2MapByCountry.Add("Anguilla", "AI");
            _countryCodeAlpha2MapByCountry.Add("Antarctica", "AQ");
            _countryCodeAlpha2MapByCountry.Add("Antigua and Barbuda", "AG");
            _countryCodeAlpha2MapByCountry.Add("Argentina", "AR");
            _countryCodeAlpha2MapByCountry.Add("Armenia", "AM");
            _countryCodeAlpha2MapByCountry.Add("Aruba", "AW");
            _countryCodeAlpha2MapByCountry.Add("Australia", "AU");
            _countryCodeAlpha2MapByCountry.Add("Austria", "AT");
            _countryCodeAlpha2MapByCountry.Add("Azerbaijan", "AZ");
            _countryCodeAlpha2MapByCountry.Add("Bahamas", "BS");
            _countryCodeAlpha2MapByCountry.Add("Bahrain", "BH");
            _countryCodeAlpha2MapByCountry.Add("Bangladesh", "BD");
            _countryCodeAlpha2MapByCountry.Add("Barbados", "BB");
            _countryCodeAlpha2MapByCountry.Add("Belarus", "BY");
            _countryCodeAlpha2MapByCountry.Add("Belgium", "BE");
            _countryCodeAlpha2MapByCountry.Add("Belize", "BZ");
            _countryCodeAlpha2MapByCountry.Add("Benin", "BJ");
            _countryCodeAlpha2MapByCountry.Add("Bermuda", "BM");
            _countryCodeAlpha2MapByCountry.Add("Bhutan", "BT");
            _countryCodeAlpha2MapByCountry.Add("Bolivia (Plurinational State of)", "BO");
            _countryCodeAlpha2MapByCountry.Add("Bonaire, Sint Eustatius and Saba", "BQ");
            _countryCodeAlpha2MapByCountry.Add("Bosnia and Herzegovina", "BA");
            _countryCodeAlpha2MapByCountry.Add("Botswana", "BW");
            _countryCodeAlpha2MapByCountry.Add("Bouvet Island", "BV");
            _countryCodeAlpha2MapByCountry.Add("Brazil", "BR");
            _countryCodeAlpha2MapByCountry.Add("British Indian Ocean Territory", "IO");
            _countryCodeAlpha2MapByCountry.Add("Brunei Darussalam", "BN");
            _countryCodeAlpha2MapByCountry.Add("Bulgaria", "BG");
            _countryCodeAlpha2MapByCountry.Add("Burkina Faso", "BF");
            _countryCodeAlpha2MapByCountry.Add("Burundi", "BI");
            _countryCodeAlpha2MapByCountry.Add("Cambodia", "KH");
            _countryCodeAlpha2MapByCountry.Add("Cameroon", "CM");
            _countryCodeAlpha2MapByCountry.Add("Canada", "CA");
            _countryCodeAlpha2MapByCountry.Add("Cabo Verde", "CV");
            _countryCodeAlpha2MapByCountry.Add("Cayman Islands", "KY");
            _countryCodeAlpha2MapByCountry.Add("Central African Republic", "CF");
            _countryCodeAlpha2MapByCountry.Add("Chad", "TD");
            _countryCodeAlpha2MapByCountry.Add("Chile", "CL");
            _countryCodeAlpha2MapByCountry.Add("China", "CN");
            _countryCodeAlpha2MapByCountry.Add("Christmas Island", "CX");
            _countryCodeAlpha2MapByCountry.Add("Cocos (Keeling) Islands", "CC");
            _countryCodeAlpha2MapByCountry.Add("Colombia", "CO");
            _countryCodeAlpha2MapByCountry.Add("Comoros", "KM");
            _countryCodeAlpha2MapByCountry.Add("Congo", "CG");
            _countryCodeAlpha2MapByCountry.Add("Congo (Democratic Republic of the)", "CD");
            _countryCodeAlpha2MapByCountry.Add("Cook Islands", "CK");
            _countryCodeAlpha2MapByCountry.Add("Costa Rica", "CR");
            _countryCodeAlpha2MapByCountry.Add("Côte d'Ivoire", "CI");
            _countryCodeAlpha2MapByCountry.Add("Croatia", "HR");
            _countryCodeAlpha2MapByCountry.Add("Cuba", "CU");
            _countryCodeAlpha2MapByCountry.Add("Curaçao", "CW");
            _countryCodeAlpha2MapByCountry.Add("Cyprus", "CY");
            _countryCodeAlpha2MapByCountry.Add("Czechia", "CZ");
            _countryCodeAlpha2MapByCountry.Add("Denmark", "DK");
            _countryCodeAlpha2MapByCountry.Add("Djibouti", "DJ");
            _countryCodeAlpha2MapByCountry.Add("Dominica", "DM");
            _countryCodeAlpha2MapByCountry.Add("Dominican Republic", "DO");
            _countryCodeAlpha2MapByCountry.Add("Ecuador", "EC");
            _countryCodeAlpha2MapByCountry.Add("Egypt", "EG");
            _countryCodeAlpha2MapByCountry.Add("El Salvador", "SV");
            _countryCodeAlpha2MapByCountry.Add("Equatorial Guinea", "GQ");
            _countryCodeAlpha2MapByCountry.Add("Eritrea", "ER");
            _countryCodeAlpha2MapByCountry.Add("Estonia", "EE");
            _countryCodeAlpha2MapByCountry.Add("Ethiopia", "ET");
            _countryCodeAlpha2MapByCountry.Add("Falkland Islands (Malvinas)", "FK");
            _countryCodeAlpha2MapByCountry.Add("Faroe Islands", "FO");
            _countryCodeAlpha2MapByCountry.Add("Fiji", "FJ");
            _countryCodeAlpha2MapByCountry.Add("Finland", "FI");
            _countryCodeAlpha2MapByCountry.Add("France", "FR");
            _countryCodeAlpha2MapByCountry.Add("French Guiana", "GF");
            _countryCodeAlpha2MapByCountry.Add("French Polynesia", "PF");
            _countryCodeAlpha2MapByCountry.Add("French Southern Territories", "TF");
            _countryCodeAlpha2MapByCountry.Add("Gabon", "GA");
            _countryCodeAlpha2MapByCountry.Add("Gambia", "GM");
            _countryCodeAlpha2MapByCountry.Add("Georgia", "GE");
            _countryCodeAlpha2MapByCountry.Add("Germany", "DE");
            _countryCodeAlpha2MapByCountry.Add("Ghana", "GH");
            _countryCodeAlpha2MapByCountry.Add("Gibraltar", "GI");
            _countryCodeAlpha2MapByCountry.Add("Greece", "GR");
            _countryCodeAlpha2MapByCountry.Add("Greenland", "GL");
            _countryCodeAlpha2MapByCountry.Add("Grenada", "GD");
            _countryCodeAlpha2MapByCountry.Add("Guadeloupe", "GP");
            _countryCodeAlpha2MapByCountry.Add("Guam", "GU");
            _countryCodeAlpha2MapByCountry.Add("Guatemala", "GT");
            _countryCodeAlpha2MapByCountry.Add("Guernsey", "GG");
            _countryCodeAlpha2MapByCountry.Add("Guinea", "GN");
            _countryCodeAlpha2MapByCountry.Add("Guinea-Bissau", "GW");
            _countryCodeAlpha2MapByCountry.Add("Guyana", "GY");
            _countryCodeAlpha2MapByCountry.Add("Haiti", "HT");
            _countryCodeAlpha2MapByCountry.Add("Heard Island and McDonald Islands", "HM");
            _countryCodeAlpha2MapByCountry.Add("Holy See", "VA");
            _countryCodeAlpha2MapByCountry.Add("Honduras", "HN");
            _countryCodeAlpha2MapByCountry.Add("Hong Kong", "HK");
            _countryCodeAlpha2MapByCountry.Add("Hungary", "HU");
            _countryCodeAlpha2MapByCountry.Add("Iceland", "IS");
            _countryCodeAlpha2MapByCountry.Add("India", "IN");
            _countryCodeAlpha2MapByCountry.Add("Indonesia", "ID");
            _countryCodeAlpha2MapByCountry.Add("Iran (Islamic Republic of)", "IR");
            _countryCodeAlpha2MapByCountry.Add("Iraq", "IQ");
            _countryCodeAlpha2MapByCountry.Add("Ireland", "IE");
            _countryCodeAlpha2MapByCountry.Add("Isle of Man", "IM");
            _countryCodeAlpha2MapByCountry.Add("Israel", "IL");
            _countryCodeAlpha2MapByCountry.Add("Italy", "IT");
            _countryCodeAlpha2MapByCountry.Add("Jamaica", "JM");
            _countryCodeAlpha2MapByCountry.Add("Japan", "JP");
            _countryCodeAlpha2MapByCountry.Add("Jersey", "JE");
            _countryCodeAlpha2MapByCountry.Add("Jordan", "JO");
            _countryCodeAlpha2MapByCountry.Add("Kazakhstan", "KZ");
            _countryCodeAlpha2MapByCountry.Add("Kenya", "KE");
            _countryCodeAlpha2MapByCountry.Add("Kiribati", "KI");
            _countryCodeAlpha2MapByCountry.Add("Korea (Democratic People's Republic of)", "KP");
            _countryCodeAlpha2MapByCountry.Add("Korea (Republic of)", "KR");
            _countryCodeAlpha2MapByCountry.Add("Kuwait", "KW");
            _countryCodeAlpha2MapByCountry.Add("Kyrgyzstan", "KG");
            _countryCodeAlpha2MapByCountry.Add("Lao People's Democratic Republic", "LA");
            _countryCodeAlpha2MapByCountry.Add("Latvia", "LV");
            _countryCodeAlpha2MapByCountry.Add("Lebanon", "LB");
            _countryCodeAlpha2MapByCountry.Add("Lesotho", "LS");
            _countryCodeAlpha2MapByCountry.Add("Liberia", "LR");
            _countryCodeAlpha2MapByCountry.Add("Libya", "LY");
            _countryCodeAlpha2MapByCountry.Add("Liechtenstein", "LI");
            _countryCodeAlpha2MapByCountry.Add("Lithuania", "LT");
            _countryCodeAlpha2MapByCountry.Add("Luxembourg", "LU");
            _countryCodeAlpha2MapByCountry.Add("Macao", "MO");
            _countryCodeAlpha2MapByCountry.Add("North Macedonia", "MK");
            _countryCodeAlpha2MapByCountry.Add("Madagascar", "MG");
            _countryCodeAlpha2MapByCountry.Add("Malawi", "MW");
            _countryCodeAlpha2MapByCountry.Add("Malaysia", "MY");
            _countryCodeAlpha2MapByCountry.Add("Maldives", "MV");
            _countryCodeAlpha2MapByCountry.Add("Mali", "ML");
            _countryCodeAlpha2MapByCountry.Add("Malta", "MT");
            _countryCodeAlpha2MapByCountry.Add("Marshall Islands", "MH");
            _countryCodeAlpha2MapByCountry.Add("Martinique", "MQ");
            _countryCodeAlpha2MapByCountry.Add("Mauritania", "MR");
            _countryCodeAlpha2MapByCountry.Add("Mauritius", "MU");
            _countryCodeAlpha2MapByCountry.Add("Mayotte", "YT");
            _countryCodeAlpha2MapByCountry.Add("Mexico", "MX");
            _countryCodeAlpha2MapByCountry.Add("Micronesia (Federated States of)", "FM");
            _countryCodeAlpha2MapByCountry.Add("Moldova (Republic of)", "MD");
            _countryCodeAlpha2MapByCountry.Add("Monaco", "MC");
            _countryCodeAlpha2MapByCountry.Add("Mongolia", "MN");
            _countryCodeAlpha2MapByCountry.Add("Montenegro", "ME");
            _countryCodeAlpha2MapByCountry.Add("Montserrat", "MS");
            _countryCodeAlpha2MapByCountry.Add("Morocco", "MA");
            _countryCodeAlpha2MapByCountry.Add("Mozambique", "MZ");
            _countryCodeAlpha2MapByCountry.Add("Myanmar", "MM");
            _countryCodeAlpha2MapByCountry.Add("Namibia", "NA");
            _countryCodeAlpha2MapByCountry.Add("Nauru", "NR");
            _countryCodeAlpha2MapByCountry.Add("Nepal", "NP");
            _countryCodeAlpha2MapByCountry.Add("Netherlands", "NL");
            _countryCodeAlpha2MapByCountry.Add("Netherlands Antilles", "AN");
            _countryCodeAlpha2MapByCountry.Add("New Caledonia", "NC");
            _countryCodeAlpha2MapByCountry.Add("New Zealand", "NZ");
            _countryCodeAlpha2MapByCountry.Add("Nicaragua", "NI");
            _countryCodeAlpha2MapByCountry.Add("Niger", "NE");
            _countryCodeAlpha2MapByCountry.Add("Nigeria", "NG");
            _countryCodeAlpha2MapByCountry.Add("Niue", "NU");
            _countryCodeAlpha2MapByCountry.Add("Norfolk Island", "NF");
            _countryCodeAlpha2MapByCountry.Add("Northern Mariana Islands", "MP");
            _countryCodeAlpha2MapByCountry.Add("Norway", "NO");
            _countryCodeAlpha2MapByCountry.Add("Oman", "OM");
            _countryCodeAlpha2MapByCountry.Add("Pakistan", "PK");
            _countryCodeAlpha2MapByCountry.Add("Palau", "PW");
            _countryCodeAlpha2MapByCountry.Add("Palestine, State of", "PS");
            _countryCodeAlpha2MapByCountry.Add("Panama", "PA");
            _countryCodeAlpha2MapByCountry.Add("Papua New Guinea", "PG");
            _countryCodeAlpha2MapByCountry.Add("Paraguay", "PY");
            _countryCodeAlpha2MapByCountry.Add("Peru", "PE");
            _countryCodeAlpha2MapByCountry.Add("Philippines", "PH");
            _countryCodeAlpha2MapByCountry.Add("Pitcairn", "PN");
            _countryCodeAlpha2MapByCountry.Add("Poland", "PL");
            _countryCodeAlpha2MapByCountry.Add("Portugal", "PT");
            _countryCodeAlpha2MapByCountry.Add("Puerto Rico", "PR");
            _countryCodeAlpha2MapByCountry.Add("Qatar", "QA");
            _countryCodeAlpha2MapByCountry.Add("Réunion", "RE");
            _countryCodeAlpha2MapByCountry.Add("Romania", "RO");
            _countryCodeAlpha2MapByCountry.Add("Russian Federation", "RU");
            _countryCodeAlpha2MapByCountry.Add("Rwanda", "RW");
            _countryCodeAlpha2MapByCountry.Add("Saint Barthélemy", "BL");
            _countryCodeAlpha2MapByCountry.Add("Saint Helena, Ascension and Tristan da Cunha", "SH");
            _countryCodeAlpha2MapByCountry.Add("Saint Kitts and Nevis", "KN");
            _countryCodeAlpha2MapByCountry.Add("Saint Lucia", "LC");
            _countryCodeAlpha2MapByCountry.Add("Saint Martin (French part)", "MF");
            _countryCodeAlpha2MapByCountry.Add("Saint Pierre and Miquelon", "PM");
            _countryCodeAlpha2MapByCountry.Add("Saint Vincent and the Grenadines", "VC");
            _countryCodeAlpha2MapByCountry.Add("Samoa", "WS");
            _countryCodeAlpha2MapByCountry.Add("San Marino", "SM");
            _countryCodeAlpha2MapByCountry.Add("Sao Tome and Principe", "ST");
            _countryCodeAlpha2MapByCountry.Add("Saudi Arabia", "SA");
            _countryCodeAlpha2MapByCountry.Add("Senegal", "SN");
            _countryCodeAlpha2MapByCountry.Add("Serbia", "RS");
            _countryCodeAlpha2MapByCountry.Add("Seychelles", "SC");
            _countryCodeAlpha2MapByCountry.Add("Sierra Leone", "SL");
            _countryCodeAlpha2MapByCountry.Add("Singapore", "SG");
            _countryCodeAlpha2MapByCountry.Add("Sint Maarten (Dutch part)", "SX");
            _countryCodeAlpha2MapByCountry.Add("Slovakia", "SK");
            _countryCodeAlpha2MapByCountry.Add("Slovenia", "SI");
            _countryCodeAlpha2MapByCountry.Add("Solomon Islands", "SB");
            _countryCodeAlpha2MapByCountry.Add("Somalia", "SO");
            _countryCodeAlpha2MapByCountry.Add("South Africa", "ZA");
            _countryCodeAlpha2MapByCountry.Add("South Georgia and the South Sandwich Islands", "GS");
            _countryCodeAlpha2MapByCountry.Add("South Sudan", "SS");
            _countryCodeAlpha2MapByCountry.Add("Spain", "ES");
            _countryCodeAlpha2MapByCountry.Add("Sri Lanka", "LK");
            _countryCodeAlpha2MapByCountry.Add("Sudan", "SD");
            _countryCodeAlpha2MapByCountry.Add("Suriname", "SR");
            _countryCodeAlpha2MapByCountry.Add("Svalbard and Jan Mayen", "SJ");
            _countryCodeAlpha2MapByCountry.Add("Eswatini", "SZ");
            _countryCodeAlpha2MapByCountry.Add("Sweden", "SE");
            _countryCodeAlpha2MapByCountry.Add("Switzerland", "CH");
            _countryCodeAlpha2MapByCountry.Add("Syrian Arab Republic", "SY");
            _countryCodeAlpha2MapByCountry.Add("Taiwan, Province of China", "TW");
            _countryCodeAlpha2MapByCountry.Add("Tajikistan", "TJ");
            _countryCodeAlpha2MapByCountry.Add("Tanzania, United Republic of", "TZ");
            _countryCodeAlpha2MapByCountry.Add("Thailand", "TH");
            _countryCodeAlpha2MapByCountry.Add("Timor-Leste", "TL");
            _countryCodeAlpha2MapByCountry.Add("Togo", "TG");
            _countryCodeAlpha2MapByCountry.Add("Tokelau", "TK");
            _countryCodeAlpha2MapByCountry.Add("Tonga", "TO");
            _countryCodeAlpha2MapByCountry.Add("Trinidad and Tobago", "TT");
            _countryCodeAlpha2MapByCountry.Add("Tunisia", "TN");
            _countryCodeAlpha2MapByCountry.Add("Turkey", "TR");
            _countryCodeAlpha2MapByCountry.Add("Turkmenistan", "TM");
            _countryCodeAlpha2MapByCountry.Add("Turks and Caicos Islands", "TC");
            _countryCodeAlpha2MapByCountry.Add("Tuvalu", "TV");
            _countryCodeAlpha2MapByCountry.Add("Uganda", "UG");
            _countryCodeAlpha2MapByCountry.Add("Ukraine", "UA");
            _countryCodeAlpha2MapByCountry.Add("United Arab Emirates", "AE");
            _countryCodeAlpha2MapByCountry.Add("United Kingdom of Great Britain and Northern Ireland", "GB");
            _countryCodeAlpha2MapByCountry.Add("United States of America", "US");
            _countryCodeAlpha2MapByCountry.Add("United States Minor Outlying Islands", "UM");
            _countryCodeAlpha2MapByCountry.Add("Uruguay", "UY");
            _countryCodeAlpha2MapByCountry.Add("Uzbekistan", "UZ");
            _countryCodeAlpha2MapByCountry.Add("Vanuatu", "VU");
            _countryCodeAlpha2MapByCountry.Add("Venezuela (Bolivarian Republic of)", "VE");
            _countryCodeAlpha2MapByCountry.Add("Vietnam", "VN");
            _countryCodeAlpha2MapByCountry.Add("Virgin Islands (British)", "VG");
            _countryCodeAlpha2MapByCountry.Add("Virgin Islands (U.S.)", "VI");
            _countryCodeAlpha2MapByCountry.Add("Wallis and Futuna", "WF");
            _countryCodeAlpha2MapByCountry.Add("Western Sahara", "EH");
            _countryCodeAlpha2MapByCountry.Add("Yemen", "YE");
            _countryCodeAlpha2MapByCountry.Add("Zambia", "ZM");
            _countryCodeAlpha2MapByCountry.Add("Zimbabwe", "ZW");

            // build the inverse
            _countryMapByAlpha2CountryCode = new Dictionary<string, string>();
            foreach (var country in _countryCodeAlpha2MapByCountry.Keys){
                _countryMapByAlpha2CountryCode.Add(_countryCodeAlpha2MapByCountry[country], country);
            }
            #endregion

            #region Alpha3byAlpha2 - Alpha2byAlpha3
            _countryCodeAlpha3MapAlpha2CountryCode = new Dictionary<string, string>();
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AF", "AFG");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AX", "ALA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AL", "ALB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("DZ", "DZA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AS", "ASM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AD", "AND");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AO", "AGO");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AI", "AIA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AQ", "ATA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AG", "ATG");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AR", "ARG");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AM", "ARM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AW", "ABW");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AU", "AUS");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AT", "AUT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AZ", "AZE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BS", "BHS");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BH", "BHR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BD", "BGD");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BB", "BRB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BY", "BLR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BE", "BEL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BZ", "BLZ");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BJ", "BEN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BM", "BMU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BT", "BTN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BO", "BOL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BQ", "BES");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BA", "BIH");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BW", "BWA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BV", "BVT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BR", "BRA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IO", "IOT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BN", "BRN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BG", "BGR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BF", "BFA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BI", "BDI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CV", "CPV");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KH", "KHM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CM", "CMR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CA", "CAN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KY", "CYM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CF", "CAF");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TD", "TCD");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CL", "CHL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CN", "CHN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CX", "CXR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CC", "CCK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CO", "COL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KM", "COM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CG", "COG");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CD", "COD");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CK", "COK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CR", "CRI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CI", "CIV");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("HR", "HRV");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CU", "CUB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CW", "CUW");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CY", "CYP");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CZ", "CZE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("DK", "DNK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("DJ", "DJI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("DM", "DMA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("DO", "DOM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("EC", "ECU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("EG", "EGY");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SV", "SLV");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GQ", "GNQ");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ER", "ERI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("EE", "EST");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SZ", "SWZ");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ET", "ETH");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("FK", "FLK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("FO", "FRO");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("FJ", "FJI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("FI", "FIN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("FR", "FRA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GF", "GUF");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PF", "PYF");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TF", "ATF");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GA", "GAB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GM", "GMB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GE", "GEO");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("DE", "DEU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GH", "GHA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GI", "GIB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GR", "GRC");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GL", "GRL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GD", "GRD");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GP", "GLP");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GU", "GUM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GT", "GTM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GG", "GGY");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GN", "GIN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GW", "GNB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GY", "GUY");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("HT", "HTI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("HM", "HMD");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("VA", "VAT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("HN", "HND");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("HK", "HKG");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("HU", "HUN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IS", "ISL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IN", "IND");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ID", "IDN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IR", "IRN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IQ", "IRQ");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IE", "IRL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IM", "IMN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IL", "ISR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("IT", "ITA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("JM", "JAM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("JP", "JPN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("JE", "JEY");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("JO", "JOR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KZ", "KAZ");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KE", "KEN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KI", "KIR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KP", "PRK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KR", "KOR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KW", "KWT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KG", "KGZ");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LA", "LAO");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LV", "LVA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LB", "LBN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LS", "LSO");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LR", "LBR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LY", "LBY");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LI", "LIE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LT", "LTU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LU", "LUX");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MO", "MAC");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MG", "MDG");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MW", "MWI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MY", "MYS");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MV", "MDV");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ML", "MLI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MT", "MLT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MH", "MHL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MQ", "MTQ");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MR", "MRT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MU", "MUS");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("YT", "MYT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MX", "MEX");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("FM", "FSM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MD", "MDA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MC", "MCO");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MN", "MNG");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ME", "MNE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MS", "MSR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MA", "MAR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MZ", "MOZ");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MM", "MMR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NA", "NAM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NR", "NRU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NP", "NPL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NL", "NLD");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NC", "NCL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NZ", "NZL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NI", "NIC");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NE", "NER");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NG", "NGA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NU", "NIU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NF", "NFK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MK", "MKD");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MP", "MNP");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("NO", "NOR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("OM", "OMN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PK", "PAK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PW", "PLW");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PS", "PSE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PA", "PAN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PG", "PNG");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PY", "PRY");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PE", "PER");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PH", "PHL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PN", "PCN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PL", "POL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PT", "PRT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PR", "PRI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("QA", "QAT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("RE", "REU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("RO", "ROU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("RU", "RUS");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("RW", "RWA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("BL", "BLM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SH", "SHN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("KN", "KNA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LC", "LCA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("MF", "MAF");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("PM", "SPM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("VC", "VCT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("WS", "WSM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SM", "SMR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ST", "STP");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SA", "SAU");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SN", "SEN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("RS", "SRB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SC", "SYC");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SL", "SLE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SG", "SGP");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SX", "SXM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SK", "SVK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SI", "SVN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SB", "SLB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SO", "SOM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ZA", "ZAF");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GS", "SGS");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SS", "SSD");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ES", "ESP");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("LK", "LKA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SD", "SDN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SR", "SUR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SJ", "SJM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SE", "SWE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("CH", "CHE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("SY", "SYR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TW", "TWN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TJ", "TJK");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TZ", "TZA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TH", "THA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TL", "TLS");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TG", "TGO");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TK", "TKL");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TO", "TON");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TT", "TTO");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TN", "TUN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TR", "TUR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TM", "TKM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TC", "TCA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("TV", "TUV");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("UG", "UGA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("UA", "UKR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("AE", "ARE");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("GB", "GBR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("US", "USA");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("UM", "UMI");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("UY", "URY");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("UZ", "UZB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("VU", "VUT");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("VE", "VEN");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("VN", "VNM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("VG", "VGB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("VI", "VIR");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("WF", "WLF");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("EH", "ESH");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("YE", "YEM");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ZM", "ZMB");
            _countryCodeAlpha3MapAlpha2CountryCode.Add("ZW", "ZWE");                        

            // build the inverse
            _countryCodeAlpha2MapAlpha3CountryCode = new Dictionary<string, string>();
            foreach (var alpha2 in _countryCodeAlpha3MapAlpha2CountryCode.Keys){
                _countryCodeAlpha2MapAlpha3CountryCode.Add(_countryCodeAlpha3MapAlpha2CountryCode[alpha2], alpha2);
            }
            #endregion

            #endregion

            #region Numeric Data Init

            #region Alpha2ByNumeric - NumericCodebyAlpha2
            _countryCodeAlpha2MapByNumericCode = new Dictionary<string, string>();
            _countryCodeAlpha2MapByNumericCode.Add("004", "AF");
            _countryCodeAlpha2MapByNumericCode.Add("008", "AL");
            _countryCodeAlpha2MapByNumericCode.Add("010", "AQ");
            _countryCodeAlpha2MapByNumericCode.Add("012", "DZ");
            _countryCodeAlpha2MapByNumericCode.Add("016", "AS");
            _countryCodeAlpha2MapByNumericCode.Add("020", "AD");
            _countryCodeAlpha2MapByNumericCode.Add("024", "AO");
            _countryCodeAlpha2MapByNumericCode.Add("028", "AG");
            _countryCodeAlpha2MapByNumericCode.Add("031", "AZ");
            _countryCodeAlpha2MapByNumericCode.Add("032", "AR");
            _countryCodeAlpha2MapByNumericCode.Add("036", "AU");
            _countryCodeAlpha2MapByNumericCode.Add("040", "AT");
            _countryCodeAlpha2MapByNumericCode.Add("044", "BS");
            _countryCodeAlpha2MapByNumericCode.Add("048", "BH");
            _countryCodeAlpha2MapByNumericCode.Add("050", "BD");
            _countryCodeAlpha2MapByNumericCode.Add("051", "AM");
            _countryCodeAlpha2MapByNumericCode.Add("052", "BB");
            _countryCodeAlpha2MapByNumericCode.Add("056", "BE");
            _countryCodeAlpha2MapByNumericCode.Add("060", "BM");
            _countryCodeAlpha2MapByNumericCode.Add("064", "BT");
            _countryCodeAlpha2MapByNumericCode.Add("068", "BO");
            _countryCodeAlpha2MapByNumericCode.Add("070", "BA");
            _countryCodeAlpha2MapByNumericCode.Add("072", "BW");
            _countryCodeAlpha2MapByNumericCode.Add("074", "BV");
            _countryCodeAlpha2MapByNumericCode.Add("076", "BR");
            _countryCodeAlpha2MapByNumericCode.Add("084", "BZ");
            _countryCodeAlpha2MapByNumericCode.Add("086", "IO");
            _countryCodeAlpha2MapByNumericCode.Add("090", "SB");
            _countryCodeAlpha2MapByNumericCode.Add("092", "VG");
            _countryCodeAlpha2MapByNumericCode.Add("096", "BN");
            _countryCodeAlpha2MapByNumericCode.Add("100", "BG");
            _countryCodeAlpha2MapByNumericCode.Add("104", "MM");
            _countryCodeAlpha2MapByNumericCode.Add("108", "BI");
            _countryCodeAlpha2MapByNumericCode.Add("112", "BY");
            _countryCodeAlpha2MapByNumericCode.Add("116", "KH");
            _countryCodeAlpha2MapByNumericCode.Add("120", "CM");
            _countryCodeAlpha2MapByNumericCode.Add("124", "CA");
            _countryCodeAlpha2MapByNumericCode.Add("132", "CV");
            _countryCodeAlpha2MapByNumericCode.Add("136", "KY");
            _countryCodeAlpha2MapByNumericCode.Add("140", "CF");
            _countryCodeAlpha2MapByNumericCode.Add("144", "LK");
            _countryCodeAlpha2MapByNumericCode.Add("148", "TD");
            _countryCodeAlpha2MapByNumericCode.Add("152", "CL");
            _countryCodeAlpha2MapByNumericCode.Add("156", "CN");
            _countryCodeAlpha2MapByNumericCode.Add("158", "TW");
            _countryCodeAlpha2MapByNumericCode.Add("162", "CX");
            _countryCodeAlpha2MapByNumericCode.Add("166", "CC");
            _countryCodeAlpha2MapByNumericCode.Add("170", "CO");
            _countryCodeAlpha2MapByNumericCode.Add("174", "KM");
            _countryCodeAlpha2MapByNumericCode.Add("175", "YT");
            _countryCodeAlpha2MapByNumericCode.Add("178", "CG");
            _countryCodeAlpha2MapByNumericCode.Add("180", "CD");
            _countryCodeAlpha2MapByNumericCode.Add("184", "CK");
            _countryCodeAlpha2MapByNumericCode.Add("188", "CR");
            _countryCodeAlpha2MapByNumericCode.Add("191", "HR");
            _countryCodeAlpha2MapByNumericCode.Add("192", "CU");
            _countryCodeAlpha2MapByNumericCode.Add("196", "CY");
            _countryCodeAlpha2MapByNumericCode.Add("203", "CZ");
            _countryCodeAlpha2MapByNumericCode.Add("204", "BJ");
            _countryCodeAlpha2MapByNumericCode.Add("208", "DK");
            _countryCodeAlpha2MapByNumericCode.Add("212", "DM");
            _countryCodeAlpha2MapByNumericCode.Add("214", "DO");
            _countryCodeAlpha2MapByNumericCode.Add("218", "EC");
            _countryCodeAlpha2MapByNumericCode.Add("222", "SV");
            _countryCodeAlpha2MapByNumericCode.Add("226", "GQ");
            _countryCodeAlpha2MapByNumericCode.Add("231", "ET");
            _countryCodeAlpha2MapByNumericCode.Add("232", "ER");
            _countryCodeAlpha2MapByNumericCode.Add("233", "EE");
            _countryCodeAlpha2MapByNumericCode.Add("234", "FO");
            _countryCodeAlpha2MapByNumericCode.Add("238", "FK");
            _countryCodeAlpha2MapByNumericCode.Add("239", "GS");
            _countryCodeAlpha2MapByNumericCode.Add("242", "FJ");
            _countryCodeAlpha2MapByNumericCode.Add("246", "FI");
            _countryCodeAlpha2MapByNumericCode.Add("248", "AX");
            _countryCodeAlpha2MapByNumericCode.Add("250", "FR");
            _countryCodeAlpha2MapByNumericCode.Add("254", "GF");
            _countryCodeAlpha2MapByNumericCode.Add("258", "PF");
            _countryCodeAlpha2MapByNumericCode.Add("260", "TF");
            _countryCodeAlpha2MapByNumericCode.Add("262", "DJ");
            _countryCodeAlpha2MapByNumericCode.Add("266", "GA");
            _countryCodeAlpha2MapByNumericCode.Add("268", "GE");
            _countryCodeAlpha2MapByNumericCode.Add("270", "GM");
            _countryCodeAlpha2MapByNumericCode.Add("275", "PS");
            _countryCodeAlpha2MapByNumericCode.Add("276", "DE");
            _countryCodeAlpha2MapByNumericCode.Add("288", "GH");
            _countryCodeAlpha2MapByNumericCode.Add("292", "GI");
            _countryCodeAlpha2MapByNumericCode.Add("296", "KI");
            _countryCodeAlpha2MapByNumericCode.Add("300", "GR");
            _countryCodeAlpha2MapByNumericCode.Add("304", "GL");
            _countryCodeAlpha2MapByNumericCode.Add("308", "GD");
            _countryCodeAlpha2MapByNumericCode.Add("312", "GP");
            _countryCodeAlpha2MapByNumericCode.Add("316", "GU");
            _countryCodeAlpha2MapByNumericCode.Add("320", "GT");
            _countryCodeAlpha2MapByNumericCode.Add("324", "GN");
            _countryCodeAlpha2MapByNumericCode.Add("328", "GY");
            _countryCodeAlpha2MapByNumericCode.Add("332", "HT");
            _countryCodeAlpha2MapByNumericCode.Add("334", "HM");
            _countryCodeAlpha2MapByNumericCode.Add("336", "VA");
            _countryCodeAlpha2MapByNumericCode.Add("340", "HN");
            _countryCodeAlpha2MapByNumericCode.Add("344", "HK");
            _countryCodeAlpha2MapByNumericCode.Add("348", "HU");
            _countryCodeAlpha2MapByNumericCode.Add("352", "IS");
            _countryCodeAlpha2MapByNumericCode.Add("356", "IN");
            _countryCodeAlpha2MapByNumericCode.Add("360", "ID");
            _countryCodeAlpha2MapByNumericCode.Add("364", "IR");
            _countryCodeAlpha2MapByNumericCode.Add("368", "IQ");
            _countryCodeAlpha2MapByNumericCode.Add("372", "IE");
            _countryCodeAlpha2MapByNumericCode.Add("376", "IL");
            _countryCodeAlpha2MapByNumericCode.Add("380", "IT");
            _countryCodeAlpha2MapByNumericCode.Add("384", "CI");
            _countryCodeAlpha2MapByNumericCode.Add("388", "JM");
            _countryCodeAlpha2MapByNumericCode.Add("392", "JP");
            _countryCodeAlpha2MapByNumericCode.Add("398", "KZ");
            _countryCodeAlpha2MapByNumericCode.Add("400", "JO");
            _countryCodeAlpha2MapByNumericCode.Add("404", "KE");
            _countryCodeAlpha2MapByNumericCode.Add("408", "KP");
            _countryCodeAlpha2MapByNumericCode.Add("410", "KR");
            _countryCodeAlpha2MapByNumericCode.Add("414", "KW");
            _countryCodeAlpha2MapByNumericCode.Add("417", "KG");
            _countryCodeAlpha2MapByNumericCode.Add("418", "LA");
            _countryCodeAlpha2MapByNumericCode.Add("422", "LB");
            _countryCodeAlpha2MapByNumericCode.Add("426", "LS");
            _countryCodeAlpha2MapByNumericCode.Add("428", "LV");
            _countryCodeAlpha2MapByNumericCode.Add("430", "LR");
            _countryCodeAlpha2MapByNumericCode.Add("434", "LY");
            _countryCodeAlpha2MapByNumericCode.Add("438", "LI");
            _countryCodeAlpha2MapByNumericCode.Add("440", "LT");
            _countryCodeAlpha2MapByNumericCode.Add("442", "LU");
            _countryCodeAlpha2MapByNumericCode.Add("446", "MO");
            _countryCodeAlpha2MapByNumericCode.Add("450", "MG");
            _countryCodeAlpha2MapByNumericCode.Add("454", "MW");
            _countryCodeAlpha2MapByNumericCode.Add("458", "MY");
            _countryCodeAlpha2MapByNumericCode.Add("462", "MV");
            _countryCodeAlpha2MapByNumericCode.Add("466", "ML");
            _countryCodeAlpha2MapByNumericCode.Add("470", "MT");
            _countryCodeAlpha2MapByNumericCode.Add("474", "MQ");
            _countryCodeAlpha2MapByNumericCode.Add("478", "MR");
            _countryCodeAlpha2MapByNumericCode.Add("480", "MU");
            _countryCodeAlpha2MapByNumericCode.Add("484", "MX");
            _countryCodeAlpha2MapByNumericCode.Add("492", "MC");
            _countryCodeAlpha2MapByNumericCode.Add("496", "MN");
            _countryCodeAlpha2MapByNumericCode.Add("498", "MD");
            _countryCodeAlpha2MapByNumericCode.Add("499", "ME");
            _countryCodeAlpha2MapByNumericCode.Add("500", "MS");
            _countryCodeAlpha2MapByNumericCode.Add("504", "MA");
            _countryCodeAlpha2MapByNumericCode.Add("508", "MZ");
            _countryCodeAlpha2MapByNumericCode.Add("512", "OM");
            _countryCodeAlpha2MapByNumericCode.Add("516", "NA");
            _countryCodeAlpha2MapByNumericCode.Add("520", "NR");
            _countryCodeAlpha2MapByNumericCode.Add("524", "NP");
            _countryCodeAlpha2MapByNumericCode.Add("528", "NL");
            _countryCodeAlpha2MapByNumericCode.Add("530", "AN");
            _countryCodeAlpha2MapByNumericCode.Add("531", "CW");
            _countryCodeAlpha2MapByNumericCode.Add("533", "AW");
            _countryCodeAlpha2MapByNumericCode.Add("534", "SX");
            _countryCodeAlpha2MapByNumericCode.Add("535", "BQ");
            _countryCodeAlpha2MapByNumericCode.Add("540", "NC");
            _countryCodeAlpha2MapByNumericCode.Add("548", "VU");
            _countryCodeAlpha2MapByNumericCode.Add("554", "NZ");
            _countryCodeAlpha2MapByNumericCode.Add("558", "NI");
            _countryCodeAlpha2MapByNumericCode.Add("562", "NE");
            _countryCodeAlpha2MapByNumericCode.Add("566", "NG");
            _countryCodeAlpha2MapByNumericCode.Add("570", "NU");
            _countryCodeAlpha2MapByNumericCode.Add("574", "NF");
            _countryCodeAlpha2MapByNumericCode.Add("578", "NO");
            _countryCodeAlpha2MapByNumericCode.Add("580", "MP");
            _countryCodeAlpha2MapByNumericCode.Add("581", "UM");
            _countryCodeAlpha2MapByNumericCode.Add("583", "FM");
            _countryCodeAlpha2MapByNumericCode.Add("584", "MH");
            _countryCodeAlpha2MapByNumericCode.Add("585", "PW");
            _countryCodeAlpha2MapByNumericCode.Add("586", "PK");
            _countryCodeAlpha2MapByNumericCode.Add("591", "PA");
            _countryCodeAlpha2MapByNumericCode.Add("598", "PG");
            _countryCodeAlpha2MapByNumericCode.Add("600", "PY");
            _countryCodeAlpha2MapByNumericCode.Add("604", "PE");
            _countryCodeAlpha2MapByNumericCode.Add("608", "PH");
            _countryCodeAlpha2MapByNumericCode.Add("612", "PN");
            _countryCodeAlpha2MapByNumericCode.Add("616", "PL");
            _countryCodeAlpha2MapByNumericCode.Add("620", "PT");
            _countryCodeAlpha2MapByNumericCode.Add("624", "GW");
            _countryCodeAlpha2MapByNumericCode.Add("626", "TL");
            _countryCodeAlpha2MapByNumericCode.Add("630", "PR");
            _countryCodeAlpha2MapByNumericCode.Add("634", "QA");
            _countryCodeAlpha2MapByNumericCode.Add("638", "RE");
            _countryCodeAlpha2MapByNumericCode.Add("642", "RO");
            _countryCodeAlpha2MapByNumericCode.Add("643", "RU");
            _countryCodeAlpha2MapByNumericCode.Add("646", "RW");
            _countryCodeAlpha2MapByNumericCode.Add("652", "BL");
            _countryCodeAlpha2MapByNumericCode.Add("654", "SH");
            _countryCodeAlpha2MapByNumericCode.Add("659", "KN");
            _countryCodeAlpha2MapByNumericCode.Add("660", "AI");
            _countryCodeAlpha2MapByNumericCode.Add("662", "LC");
            _countryCodeAlpha2MapByNumericCode.Add("663", "MF");
            _countryCodeAlpha2MapByNumericCode.Add("666", "PM");
            _countryCodeAlpha2MapByNumericCode.Add("670", "VC");
            _countryCodeAlpha2MapByNumericCode.Add("674", "SM");
            _countryCodeAlpha2MapByNumericCode.Add("678", "ST");
            _countryCodeAlpha2MapByNumericCode.Add("682", "SA");
            _countryCodeAlpha2MapByNumericCode.Add("686", "SN");
            _countryCodeAlpha2MapByNumericCode.Add("688", "RS");
            _countryCodeAlpha2MapByNumericCode.Add("690", "SC");
            _countryCodeAlpha2MapByNumericCode.Add("694", "SL");
            _countryCodeAlpha2MapByNumericCode.Add("702", "SG");
            _countryCodeAlpha2MapByNumericCode.Add("703", "SK");
            _countryCodeAlpha2MapByNumericCode.Add("704", "VN");
            _countryCodeAlpha2MapByNumericCode.Add("705", "SI");
            _countryCodeAlpha2MapByNumericCode.Add("706", "SO");
            _countryCodeAlpha2MapByNumericCode.Add("710", "ZA");
            _countryCodeAlpha2MapByNumericCode.Add("716", "ZW");
            _countryCodeAlpha2MapByNumericCode.Add("724", "ES");
            _countryCodeAlpha2MapByNumericCode.Add("728", "SS");
            _countryCodeAlpha2MapByNumericCode.Add("729", "SD");
            _countryCodeAlpha2MapByNumericCode.Add("732", "EH");
            _countryCodeAlpha2MapByNumericCode.Add("740", "SR");
            _countryCodeAlpha2MapByNumericCode.Add("744", "SJ");
            _countryCodeAlpha2MapByNumericCode.Add("748", "SZ");
            _countryCodeAlpha2MapByNumericCode.Add("752", "SE");
            _countryCodeAlpha2MapByNumericCode.Add("756", "CH");
            _countryCodeAlpha2MapByNumericCode.Add("760", "SY");
            _countryCodeAlpha2MapByNumericCode.Add("762", "TJ");
            _countryCodeAlpha2MapByNumericCode.Add("764", "TH");
            _countryCodeAlpha2MapByNumericCode.Add("768", "TG");
            _countryCodeAlpha2MapByNumericCode.Add("772", "TK");
            _countryCodeAlpha2MapByNumericCode.Add("776", "TO");
            _countryCodeAlpha2MapByNumericCode.Add("780", "TT");
            _countryCodeAlpha2MapByNumericCode.Add("784", "AE");
            _countryCodeAlpha2MapByNumericCode.Add("788", "TN");
            _countryCodeAlpha2MapByNumericCode.Add("792", "TR");
            _countryCodeAlpha2MapByNumericCode.Add("795", "TM");
            _countryCodeAlpha2MapByNumericCode.Add("796", "TC");
            _countryCodeAlpha2MapByNumericCode.Add("798", "TV");
            _countryCodeAlpha2MapByNumericCode.Add("800", "UG");
            _countryCodeAlpha2MapByNumericCode.Add("804", "UA");
            _countryCodeAlpha2MapByNumericCode.Add("807", "MK");
            _countryCodeAlpha2MapByNumericCode.Add("818", "EG");
            _countryCodeAlpha2MapByNumericCode.Add("826", "GB");
            _countryCodeAlpha2MapByNumericCode.Add("831", "GG");
            _countryCodeAlpha2MapByNumericCode.Add("832", "JE");
            _countryCodeAlpha2MapByNumericCode.Add("833", "IM");
            _countryCodeAlpha2MapByNumericCode.Add("834", "TZ");
            _countryCodeAlpha2MapByNumericCode.Add("840", "US");
            _countryCodeAlpha2MapByNumericCode.Add("850", "VI");
            _countryCodeAlpha2MapByNumericCode.Add("854", "BF");
            _countryCodeAlpha2MapByNumericCode.Add("858", "UY");
            _countryCodeAlpha2MapByNumericCode.Add("860", "UZ");
            _countryCodeAlpha2MapByNumericCode.Add("862", "VE");
            _countryCodeAlpha2MapByNumericCode.Add("876", "WF");
            _countryCodeAlpha2MapByNumericCode.Add("882", "WS");
            _countryCodeAlpha2MapByNumericCode.Add("887", "YE");
            _countryCodeAlpha2MapByNumericCode.Add("894", "ZM");

            // build the inverse
            _numericCodeMapByCountryCodeAlpha2 = new Dictionary<string, string>();
            foreach (var numericCode in _countryCodeAlpha2MapByNumericCode.Keys){
                _numericCodeMapByCountryCodeAlpha2.Add(_countryCodeAlpha2MapByNumericCode[numericCode], numericCode);
            }

            #endregion

            #region NumericByCountryName - CountryNameByNumeric
            _numericCodeMapByCountry = new Dictionary<string, string>();
            _numericCodeMapByCountry.Add("Afghanistan", "004");
            _numericCodeMapByCountry.Add("Åland Islands", "248");
            _numericCodeMapByCountry.Add("Albania", "008");
            _numericCodeMapByCountry.Add("Algeria", "012");
            _numericCodeMapByCountry.Add("American Samoa", "016");
            _numericCodeMapByCountry.Add("Andorra", "020");
            _numericCodeMapByCountry.Add("Angola", "024");
            _numericCodeMapByCountry.Add("Anguilla", "660");
            _numericCodeMapByCountry.Add("Antarctica", "010");
            _numericCodeMapByCountry.Add("Antigua and Barbuda", "028");
            _numericCodeMapByCountry.Add("Argentina", "032");
            _numericCodeMapByCountry.Add("Armenia", "051");
            _numericCodeMapByCountry.Add("Aruba", "533");
            _numericCodeMapByCountry.Add("Australia", "036");
            _numericCodeMapByCountry.Add("Austria", "040");
            _numericCodeMapByCountry.Add("Azerbaijan", "031");
            _numericCodeMapByCountry.Add("Bahamas", "044");
            _numericCodeMapByCountry.Add("Bahrain", "048");
            _numericCodeMapByCountry.Add("Bangladesh", "050");
            _numericCodeMapByCountry.Add("Barbados", "052");
            _numericCodeMapByCountry.Add("Belarus", "112");
            _numericCodeMapByCountry.Add("Belgium", "056");
            _numericCodeMapByCountry.Add("Belize", "084");
            _numericCodeMapByCountry.Add("Benin", "204");
            _numericCodeMapByCountry.Add("Bermuda", "060");
            _numericCodeMapByCountry.Add("Bhutan", "064");
            _numericCodeMapByCountry.Add("Bolivia (Plurinational State of)", "068");
            _numericCodeMapByCountry.Add("Bonaire, Sint Eustatius and Saba", "535");
            _numericCodeMapByCountry.Add("Bosnia and Herzegovina", "070");
            _numericCodeMapByCountry.Add("Botswana", "072");
            _numericCodeMapByCountry.Add("Bouvet Island", "074");
            _numericCodeMapByCountry.Add("Brazil", "076");
            _numericCodeMapByCountry.Add("British Indian Ocean Territory", "086");
            _numericCodeMapByCountry.Add("Brunei Darussalam", "096");
            _numericCodeMapByCountry.Add("Bulgaria", "100");
            _numericCodeMapByCountry.Add("Burkina Faso", "854");
            _numericCodeMapByCountry.Add("Burundi", "108");
            _numericCodeMapByCountry.Add("Cabo Verde", "132");
            _numericCodeMapByCountry.Add("Cambodia", "116");
            _numericCodeMapByCountry.Add("Cameroon", "120");
            _numericCodeMapByCountry.Add("Canada", "124");
            _numericCodeMapByCountry.Add("Cayman Islands", "136");
            _numericCodeMapByCountry.Add("Central African Republic", "140");
            _numericCodeMapByCountry.Add("Chad", "148");
            _numericCodeMapByCountry.Add("Chile", "152");
            _numericCodeMapByCountry.Add("China", "156");
            _numericCodeMapByCountry.Add("Christmas Island", "162");
            _numericCodeMapByCountry.Add("Cocos (Keeling) Islands", "166");
            _numericCodeMapByCountry.Add("Colombia", "170");
            _numericCodeMapByCountry.Add("Comoros", "174");
            _numericCodeMapByCountry.Add("Congo", "178");
            _numericCodeMapByCountry.Add("Congo, Democratic Republic of the", "180");
            _numericCodeMapByCountry.Add("Cook Islands", "184");
            _numericCodeMapByCountry.Add("Costa Rica", "188");
            _numericCodeMapByCountry.Add("Côte d'Ivoire", "384");
            _numericCodeMapByCountry.Add("Croatia", "191");
            _numericCodeMapByCountry.Add("Cuba", "192");
            _numericCodeMapByCountry.Add("Curaçao", "531");
            _numericCodeMapByCountry.Add("Cyprus", "196");
            _numericCodeMapByCountry.Add("Czechia", "203");
            _numericCodeMapByCountry.Add("Denmark", "208");
            _numericCodeMapByCountry.Add("Djibouti", "262");
            _numericCodeMapByCountry.Add("Dominica", "212");
            _numericCodeMapByCountry.Add("Dominican Republic", "214");
            _numericCodeMapByCountry.Add("Ecuador", "218");
            _numericCodeMapByCountry.Add("Egypt", "818");
            _numericCodeMapByCountry.Add("El Salvador", "222");
            _numericCodeMapByCountry.Add("Equatorial Guinea", "226");
            _numericCodeMapByCountry.Add("Eritrea", "232");
            _numericCodeMapByCountry.Add("Estonia", "233");
            _numericCodeMapByCountry.Add("Eswatini", "748");
            _numericCodeMapByCountry.Add("Ethiopia", "231");
            _numericCodeMapByCountry.Add("Falkland Islands (Malvinas)", "238");
            _numericCodeMapByCountry.Add("Faroe Islands", "234");
            _numericCodeMapByCountry.Add("Fiji", "242");
            _numericCodeMapByCountry.Add("Finland", "246");
            _numericCodeMapByCountry.Add("France", "250");
            _numericCodeMapByCountry.Add("French Guiana", "254");
            _numericCodeMapByCountry.Add("French Polynesia", "258");
            _numericCodeMapByCountry.Add("French Southern Territories", "260");
            _numericCodeMapByCountry.Add("Gabon", "266");
            _numericCodeMapByCountry.Add("Gambia", "270");
            _numericCodeMapByCountry.Add("Georgia", "268");
            _numericCodeMapByCountry.Add("Germany", "276");
            _numericCodeMapByCountry.Add("Ghana", "288");
            _numericCodeMapByCountry.Add("Gibraltar", "292");
            _numericCodeMapByCountry.Add("Greece", "300");
            _numericCodeMapByCountry.Add("Greenland", "304");
            _numericCodeMapByCountry.Add("Grenada", "308");
            _numericCodeMapByCountry.Add("Guadeloupe", "312");
            _numericCodeMapByCountry.Add("Guam", "316");
            _numericCodeMapByCountry.Add("Guatemala", "320");
            _numericCodeMapByCountry.Add("Guernsey", "831");
            _numericCodeMapByCountry.Add("Guinea", "324");
            _numericCodeMapByCountry.Add("Guinea-Bissau", "624");
            _numericCodeMapByCountry.Add("Guyana", "328");
            _numericCodeMapByCountry.Add("Haiti", "332");
            _numericCodeMapByCountry.Add("Heard Island and McDonald Islands", "334");
            _numericCodeMapByCountry.Add("Holy See", "336");
            _numericCodeMapByCountry.Add("Honduras", "340");
            _numericCodeMapByCountry.Add("Hong Kong", "344");
            _numericCodeMapByCountry.Add("Hungary", "348");
            _numericCodeMapByCountry.Add("Iceland", "352");
            _numericCodeMapByCountry.Add("India", "356");
            _numericCodeMapByCountry.Add("Indonesia", "360");
            _numericCodeMapByCountry.Add("Iran (Islamic Republic of)", "364");
            _numericCodeMapByCountry.Add("Iraq", "368");
            _numericCodeMapByCountry.Add("Ireland", "372");
            _numericCodeMapByCountry.Add("Isle of Man", "833");
            _numericCodeMapByCountry.Add("Israel", "376");
            _numericCodeMapByCountry.Add("Italy", "380");
            _numericCodeMapByCountry.Add("Jamaica", "388");
            _numericCodeMapByCountry.Add("Japan", "392");
            _numericCodeMapByCountry.Add("Jersey", "832");
            _numericCodeMapByCountry.Add("Jordan", "400");
            _numericCodeMapByCountry.Add("Kazakhstan", "398");
            _numericCodeMapByCountry.Add("Kenya", "404");
            _numericCodeMapByCountry.Add("Kiribati", "296");
            _numericCodeMapByCountry.Add("Korea (Democratic People's Republic of)", "408");
            _numericCodeMapByCountry.Add("Korea, Republic of", "410");
            _numericCodeMapByCountry.Add("Kuwait", "414");
            _numericCodeMapByCountry.Add("Kyrgyzstan", "417");
            _numericCodeMapByCountry.Add("Lao People's Democratic Republic", "418");
            _numericCodeMapByCountry.Add("Latvia", "428");
            _numericCodeMapByCountry.Add("Lebanon", "422");
            _numericCodeMapByCountry.Add("Lesotho", "426");
            _numericCodeMapByCountry.Add("Liberia", "430");
            _numericCodeMapByCountry.Add("Libya", "434");
            _numericCodeMapByCountry.Add("Liechtenstein", "438");
            _numericCodeMapByCountry.Add("Lithuania", "440");
            _numericCodeMapByCountry.Add("Luxembourg", "442");
            _numericCodeMapByCountry.Add("Macao", "446");
            _numericCodeMapByCountry.Add("Madagascar", "450");
            _numericCodeMapByCountry.Add("Malawi", "454");
            _numericCodeMapByCountry.Add("Malaysia", "458");
            _numericCodeMapByCountry.Add("Maldives", "462");
            _numericCodeMapByCountry.Add("Mali", "466");
            _numericCodeMapByCountry.Add("Malta", "470");
            _numericCodeMapByCountry.Add("Marshall Islands", "584");
            _numericCodeMapByCountry.Add("Martinique", "474");
            _numericCodeMapByCountry.Add("Mauritania", "478");
            _numericCodeMapByCountry.Add("Mauritius", "480");
            _numericCodeMapByCountry.Add("Mayotte", "175");
            _numericCodeMapByCountry.Add("Mexico", "484");
            _numericCodeMapByCountry.Add("Micronesia (Federated States of)", "583");
            _numericCodeMapByCountry.Add("Moldova, Republic of", "498");
            _numericCodeMapByCountry.Add("Monaco", "492");
            _numericCodeMapByCountry.Add("Mongolia", "496");
            _numericCodeMapByCountry.Add("Montenegro", "499");
            _numericCodeMapByCountry.Add("Montserrat", "500");
            _numericCodeMapByCountry.Add("Morocco", "504");
            _numericCodeMapByCountry.Add("Mozambique", "508");
            _numericCodeMapByCountry.Add("Myanmar", "104");
            _numericCodeMapByCountry.Add("Namibia", "516");
            _numericCodeMapByCountry.Add("Nauru", "520");
            _numericCodeMapByCountry.Add("Nepal", "524");
            _numericCodeMapByCountry.Add("Netherlands", "528");
            _numericCodeMapByCountry.Add("New Caledonia", "540");
            _numericCodeMapByCountry.Add("New Zealand", "554");
            _numericCodeMapByCountry.Add("Nicaragua", "558");
            _numericCodeMapByCountry.Add("Niger", "562");
            _numericCodeMapByCountry.Add("Nigeria", "566");
            _numericCodeMapByCountry.Add("Niue", "570");
            _numericCodeMapByCountry.Add("Norfolk Island", "574");
            _numericCodeMapByCountry.Add("North Macedonia", "807");
            _numericCodeMapByCountry.Add("Northern Mariana Islands", "580");
            _numericCodeMapByCountry.Add("Norway", "578");
            _numericCodeMapByCountry.Add("Oman", "512");
            _numericCodeMapByCountry.Add("Pakistan", "586");
            _numericCodeMapByCountry.Add("Palau", "585");
            _numericCodeMapByCountry.Add("Palestine, State of", "275");
            _numericCodeMapByCountry.Add("Panama", "591");
            _numericCodeMapByCountry.Add("Papua New Guinea", "598");
            _numericCodeMapByCountry.Add("Paraguay", "600");
            _numericCodeMapByCountry.Add("Peru", "604");
            _numericCodeMapByCountry.Add("Philippines", "608");
            _numericCodeMapByCountry.Add("Pitcairn", "612");
            _numericCodeMapByCountry.Add("Poland", "616");
            _numericCodeMapByCountry.Add("Portugal", "620");
            _numericCodeMapByCountry.Add("Puerto Rico", "630");
            _numericCodeMapByCountry.Add("Qatar", "634");
            _numericCodeMapByCountry.Add("Réunion", "638");
            _numericCodeMapByCountry.Add("Romania", "642");
            _numericCodeMapByCountry.Add("Russian Federation", "643");
            _numericCodeMapByCountry.Add("Rwanda", "646");
            _numericCodeMapByCountry.Add("Saint Barthélemy", "652");
            _numericCodeMapByCountry.Add("Saint Helena, Ascension and Tristan da Cunha", "654");
            _numericCodeMapByCountry.Add("Saint Kitts and Nevis", "659");
            _numericCodeMapByCountry.Add("Saint Lucia", "662");
            _numericCodeMapByCountry.Add("Saint Martin (French part)", "663");
            _numericCodeMapByCountry.Add("Saint Pierre and Miquelon", "666");
            _numericCodeMapByCountry.Add("Saint Vincent and the Grenadines", "670");
            _numericCodeMapByCountry.Add("Samoa", "882");
            _numericCodeMapByCountry.Add("San Marino", "674");
            _numericCodeMapByCountry.Add("Sao Tome and Principe", "678");
            _numericCodeMapByCountry.Add("Saudi Arabia", "682");
            _numericCodeMapByCountry.Add("Senegal", "686");
            _numericCodeMapByCountry.Add("Serbia", "688");
            _numericCodeMapByCountry.Add("Seychelles", "690");
            _numericCodeMapByCountry.Add("Sierra Leone", "694");
            _numericCodeMapByCountry.Add("Singapore", "702");
            _numericCodeMapByCountry.Add("Sint Maarten (Dutch part)", "534");
            _numericCodeMapByCountry.Add("Slovakia", "703");
            _numericCodeMapByCountry.Add("Slovenia", "705");
            _numericCodeMapByCountry.Add("Solomon Islands", "090");
            _numericCodeMapByCountry.Add("Somalia", "706");
            _numericCodeMapByCountry.Add("South Africa", "710");
            _numericCodeMapByCountry.Add("South Georgia and the South Sandwich Islands", "239");
            _numericCodeMapByCountry.Add("South Sudan", "728");
            _numericCodeMapByCountry.Add("Spain", "724");
            _numericCodeMapByCountry.Add("Sri Lanka", "144");
            _numericCodeMapByCountry.Add("Sudan", "729");
            _numericCodeMapByCountry.Add("Suriname", "740");
            _numericCodeMapByCountry.Add("Svalbard and Jan Mayen", "744");
            _numericCodeMapByCountry.Add("Sweden", "752");
            _numericCodeMapByCountry.Add("Switzerland", "756");
            _numericCodeMapByCountry.Add("Syrian Arab Republic", "760");
            _numericCodeMapByCountry.Add("Taiwan, Province of China", "158");
            _numericCodeMapByCountry.Add("Tajikistan", "762");
            _numericCodeMapByCountry.Add("Tanzania, United Republic of", "834");
            _numericCodeMapByCountry.Add("Thailand", "764");
            _numericCodeMapByCountry.Add("Timor-Leste", "626");
            _numericCodeMapByCountry.Add("Togo", "768");
            _numericCodeMapByCountry.Add("Tokelau", "772");
            _numericCodeMapByCountry.Add("Tonga", "776");
            _numericCodeMapByCountry.Add("Trinidad and Tobago", "780");
            _numericCodeMapByCountry.Add("Tunisia", "788");
            _numericCodeMapByCountry.Add("Turkey", "792");
            _numericCodeMapByCountry.Add("Turkmenistan", "795");
            _numericCodeMapByCountry.Add("Turks and Caicos Islands", "796");
            _numericCodeMapByCountry.Add("Tuvalu", "798");
            _numericCodeMapByCountry.Add("Uganda", "800");
            _numericCodeMapByCountry.Add("Ukraine", "804");
            _numericCodeMapByCountry.Add("United Arab Emirates", "784");
            _numericCodeMapByCountry.Add("United Kingdom of Great Britain and Northern Ireland", "826");
            _numericCodeMapByCountry.Add("United States of America", "840");
            _numericCodeMapByCountry.Add("United States Minor Outlying Islands", "581");
            _numericCodeMapByCountry.Add("Uruguay", "858");
            _numericCodeMapByCountry.Add("Uzbekistan", "860");
            _numericCodeMapByCountry.Add("Vanuatu", "548");
            _numericCodeMapByCountry.Add("Venezuela (Bolivarian Republic of)", "862");
            _numericCodeMapByCountry.Add("Vietnam", "704");
            _numericCodeMapByCountry.Add("Virgin Islands (British)", "092");
            _numericCodeMapByCountry.Add("Virgin Islands (U.S.)", "850");
            _numericCodeMapByCountry.Add("Wallis and Futuna", "876");
            _numericCodeMapByCountry.Add("Western Sahara", "732");
            _numericCodeMapByCountry.Add("Yemen", "887");
            _numericCodeMapByCountry.Add("Zambia", "894");
            _numericCodeMapByCountry.Add("Zimbabwe", "716");                    

            // build the inverse
            _countryMapByNumeric = new Dictionary<string, string>();
            foreach (var country in _numericCodeMapByCountry.Keys){
                _countryMapByNumeric.Add(_numericCodeMapByCountry[country], country);
            }

            #endregion

            #endregion

            #region Alpha 3 Data Init

            #region Alpha3ByCountryName - CountryNamebyAlpha3
            _countryCodeAlpha3MapByCountry = new Dictionary<string, string>();
            _countryCodeAlpha3MapByCountry.Add("Afghanistan", "AFG");
            _countryCodeAlpha3MapByCountry.Add("Åland Islands", "ALA");
            _countryCodeAlpha3MapByCountry.Add("Albania", "ALB");
            _countryCodeAlpha3MapByCountry.Add("Algeria", "DZA");
            _countryCodeAlpha3MapByCountry.Add("American Samoa", "ASM");
            _countryCodeAlpha3MapByCountry.Add("Andorra", "AND");
            _countryCodeAlpha3MapByCountry.Add("Angola", "AGO");
            _countryCodeAlpha3MapByCountry.Add("Anguilla", "AIA");
            _countryCodeAlpha3MapByCountry.Add("Antarctica", "ATA");
            _countryCodeAlpha3MapByCountry.Add("Antigua and Barbuda", "ATG");
            _countryCodeAlpha3MapByCountry.Add("Argentina", "ARG");
            _countryCodeAlpha3MapByCountry.Add("Armenia", "ARM");
            _countryCodeAlpha3MapByCountry.Add("Aruba", "ABW");
            _countryCodeAlpha3MapByCountry.Add("Australia", "AUS");
            _countryCodeAlpha3MapByCountry.Add("Austria", "AUT");
            _countryCodeAlpha3MapByCountry.Add("Azerbaijan", "AZE");
            _countryCodeAlpha3MapByCountry.Add("Bahamas", "BHS");
            _countryCodeAlpha3MapByCountry.Add("Bahrain", "BHR");
            _countryCodeAlpha3MapByCountry.Add("Bangladesh", "BGD");
            _countryCodeAlpha3MapByCountry.Add("Barbados", "BRB");
            _countryCodeAlpha3MapByCountry.Add("Belarus", "BLR");
            _countryCodeAlpha3MapByCountry.Add("Belgium", "BEL");
            _countryCodeAlpha3MapByCountry.Add("Belize", "BLZ");
            _countryCodeAlpha3MapByCountry.Add("Benin", "BEN");
            _countryCodeAlpha3MapByCountry.Add("Bermuda", "BMU");
            _countryCodeAlpha3MapByCountry.Add("Bhutan", "BTN");
            _countryCodeAlpha3MapByCountry.Add("Bolivia (Plurinational State of)", "BOL");
            _countryCodeAlpha3MapByCountry.Add("Bonaire, Sint Eustatius and Saba", "BES");
            _countryCodeAlpha3MapByCountry.Add("Bosnia and Herzegovina", "BIH");
            _countryCodeAlpha3MapByCountry.Add("Botswana", "BWA");
            _countryCodeAlpha3MapByCountry.Add("Bouvet Island", "BVT");
            _countryCodeAlpha3MapByCountry.Add("Brazil", "BRA");
            _countryCodeAlpha3MapByCountry.Add("British Indian Ocean Territory", "IOT");
            _countryCodeAlpha3MapByCountry.Add("Brunei Darussalam", "BRN");
            _countryCodeAlpha3MapByCountry.Add("Bulgaria", "BGR");
            _countryCodeAlpha3MapByCountry.Add("Burkina Faso", "BFA");
            _countryCodeAlpha3MapByCountry.Add("Burundi", "BDI");
            _countryCodeAlpha3MapByCountry.Add("Cabo Verde", "CPV");
            _countryCodeAlpha3MapByCountry.Add("Cambodia", "KHM");
            _countryCodeAlpha3MapByCountry.Add("Cameroon", "CMR");
            _countryCodeAlpha3MapByCountry.Add("Canada", "CAN");
            _countryCodeAlpha3MapByCountry.Add("Cayman Islands", "CYM");
            _countryCodeAlpha3MapByCountry.Add("Central African Republic", "CAF");
            _countryCodeAlpha3MapByCountry.Add("Chad", "TCD");
            _countryCodeAlpha3MapByCountry.Add("Chile", "CHL");
            _countryCodeAlpha3MapByCountry.Add("China", "CHN");
            _countryCodeAlpha3MapByCountry.Add("Christmas Island", "CXR");
            _countryCodeAlpha3MapByCountry.Add("Cocos (Keeling) Islands", "CCK");
            _countryCodeAlpha3MapByCountry.Add("Colombia", "COL");
            _countryCodeAlpha3MapByCountry.Add("Comoros", "COM");
            _countryCodeAlpha3MapByCountry.Add("Congo", "COG");
            _countryCodeAlpha3MapByCountry.Add("Congo, Democratic Republic of the", "COD");
            _countryCodeAlpha3MapByCountry.Add("Cook Islands", "COK");
            _countryCodeAlpha3MapByCountry.Add("Costa Rica", "CRI");
            _countryCodeAlpha3MapByCountry.Add("Côte d'Ivoire", "CIV");
            _countryCodeAlpha3MapByCountry.Add("Croatia", "HRV");
            _countryCodeAlpha3MapByCountry.Add("Cuba", "CUB");
            _countryCodeAlpha3MapByCountry.Add("Curaçao", "CUW");
            _countryCodeAlpha3MapByCountry.Add("Cyprus", "CYP");
            _countryCodeAlpha3MapByCountry.Add("Czechia", "CZE");
            _countryCodeAlpha3MapByCountry.Add("Denmark", "DNK");
            _countryCodeAlpha3MapByCountry.Add("Djibouti", "DJI");
            _countryCodeAlpha3MapByCountry.Add("Dominica", "DMA");
            _countryCodeAlpha3MapByCountry.Add("Dominican Republic", "DOM");
            _countryCodeAlpha3MapByCountry.Add("Ecuador", "ECU");
            _countryCodeAlpha3MapByCountry.Add("Egypt", "EGY");
            _countryCodeAlpha3MapByCountry.Add("El Salvador", "SLV");
            _countryCodeAlpha3MapByCountry.Add("Equatorial Guinea", "GNQ");
            _countryCodeAlpha3MapByCountry.Add("Eritrea", "ERI");
            _countryCodeAlpha3MapByCountry.Add("Estonia", "EST");
            _countryCodeAlpha3MapByCountry.Add("Eswatini", "SWZ");
            _countryCodeAlpha3MapByCountry.Add("Ethiopia", "ETH");
            _countryCodeAlpha3MapByCountry.Add("Falkland Islands (Malvinas)", "FLK");
            _countryCodeAlpha3MapByCountry.Add("Faroe Islands", "FRO");
            _countryCodeAlpha3MapByCountry.Add("Fiji", "FJI");
            _countryCodeAlpha3MapByCountry.Add("Finland", "FIN");
            _countryCodeAlpha3MapByCountry.Add("France", "FRA");
            _countryCodeAlpha3MapByCountry.Add("French Guiana", "GUF");
            _countryCodeAlpha3MapByCountry.Add("French Polynesia", "PYF");
            _countryCodeAlpha3MapByCountry.Add("French Southern Territories", "ATF");
            _countryCodeAlpha3MapByCountry.Add("Gabon", "GAB");
            _countryCodeAlpha3MapByCountry.Add("Gambia", "GMB");
            _countryCodeAlpha3MapByCountry.Add("Georgia", "GEO");
            _countryCodeAlpha3MapByCountry.Add("Germany", "DEU");
            _countryCodeAlpha3MapByCountry.Add("Ghana", "GHA");
            _countryCodeAlpha3MapByCountry.Add("Gibraltar", "GIB");
            _countryCodeAlpha3MapByCountry.Add("Greece", "GRC");
            _countryCodeAlpha3MapByCountry.Add("Greenland", "GRL");
            _countryCodeAlpha3MapByCountry.Add("Grenada", "GRD");
            _countryCodeAlpha3MapByCountry.Add("Guadeloupe", "GLP");
            _countryCodeAlpha3MapByCountry.Add("Guam", "GUM");
            _countryCodeAlpha3MapByCountry.Add("Guatemala", "GTM");
            _countryCodeAlpha3MapByCountry.Add("Guernsey", "GGY");
            _countryCodeAlpha3MapByCountry.Add("Guinea", "GIN");
            _countryCodeAlpha3MapByCountry.Add("Guinea-Bissau", "GNB");
            _countryCodeAlpha3MapByCountry.Add("Guyana", "GUY");
            _countryCodeAlpha3MapByCountry.Add("Haiti", "HTI");
            _countryCodeAlpha3MapByCountry.Add("Heard Island and McDonald Islands", "HMD");
            _countryCodeAlpha3MapByCountry.Add("Holy See", "VAT");
            _countryCodeAlpha3MapByCountry.Add("Honduras", "HND");
            _countryCodeAlpha3MapByCountry.Add("Hong Kong", "HKG");
            _countryCodeAlpha3MapByCountry.Add("Hungary", "HUN");
            _countryCodeAlpha3MapByCountry.Add("Iceland", "ISL");
            _countryCodeAlpha3MapByCountry.Add("India", "IND");
            _countryCodeAlpha3MapByCountry.Add("Indonesia", "IDN");
            _countryCodeAlpha3MapByCountry.Add("Iran (Islamic Republic of)", "IRN");
            _countryCodeAlpha3MapByCountry.Add("Iraq", "IRQ");
            _countryCodeAlpha3MapByCountry.Add("Ireland", "IRL");
            _countryCodeAlpha3MapByCountry.Add("Isle of Man", "IMN");
            _countryCodeAlpha3MapByCountry.Add("Israel", "ISR");
            _countryCodeAlpha3MapByCountry.Add("Italy", "ITA");
            _countryCodeAlpha3MapByCountry.Add("Jamaica", "JAM");
            _countryCodeAlpha3MapByCountry.Add("Japan", "JPN");
            _countryCodeAlpha3MapByCountry.Add("Jersey", "JEY");
            _countryCodeAlpha3MapByCountry.Add("Jordan", "JOR");
            _countryCodeAlpha3MapByCountry.Add("Kazakhstan", "KAZ");
            _countryCodeAlpha3MapByCountry.Add("Kenya", "KEN");
            _countryCodeAlpha3MapByCountry.Add("Kiribati", "KIR");
            _countryCodeAlpha3MapByCountry.Add("Korea (Democratic People's Republic of)", "PRK");
            _countryCodeAlpha3MapByCountry.Add("Korea, Republic of", "KOR");
            _countryCodeAlpha3MapByCountry.Add("Kuwait", "KWT");
            _countryCodeAlpha3MapByCountry.Add("Kyrgyzstan", "KGZ");
            _countryCodeAlpha3MapByCountry.Add("Lao People's Democratic Republic", "LAO");
            _countryCodeAlpha3MapByCountry.Add("Latvia", "LVA");
            _countryCodeAlpha3MapByCountry.Add("Lebanon", "LBN");
            _countryCodeAlpha3MapByCountry.Add("Lesotho", "LSO");
            _countryCodeAlpha3MapByCountry.Add("Liberia", "LBR");
            _countryCodeAlpha3MapByCountry.Add("Libya", "LBY");
            _countryCodeAlpha3MapByCountry.Add("Liechtenstein", "LIE");
            _countryCodeAlpha3MapByCountry.Add("Lithuania", "LTU");
            _countryCodeAlpha3MapByCountry.Add("Luxembourg", "LUX");
            _countryCodeAlpha3MapByCountry.Add("Macao", "MAC");
            _countryCodeAlpha3MapByCountry.Add("Madagascar", "MDG");
            _countryCodeAlpha3MapByCountry.Add("Malawi", "MWI");
            _countryCodeAlpha3MapByCountry.Add("Malaysia", "MYS");
            _countryCodeAlpha3MapByCountry.Add("Maldives", "MDV");
            _countryCodeAlpha3MapByCountry.Add("Mali", "MLI");
            _countryCodeAlpha3MapByCountry.Add("Malta", "MLT");
            _countryCodeAlpha3MapByCountry.Add("Marshall Islands", "MHL");
            _countryCodeAlpha3MapByCountry.Add("Martinique", "MTQ");
            _countryCodeAlpha3MapByCountry.Add("Mauritania", "MRT");
            _countryCodeAlpha3MapByCountry.Add("Mauritius", "MUS");
            _countryCodeAlpha3MapByCountry.Add("Mayotte", "MYT");
            _countryCodeAlpha3MapByCountry.Add("Mexico", "MEX");
            _countryCodeAlpha3MapByCountry.Add("Micronesia (Federated States of)", "FSM");
            _countryCodeAlpha3MapByCountry.Add("Moldova, Republic of", "MDA");
            _countryCodeAlpha3MapByCountry.Add("Monaco", "MCO");
            _countryCodeAlpha3MapByCountry.Add("Mongolia", "MNG");
            _countryCodeAlpha3MapByCountry.Add("Montenegro", "MNE");
            _countryCodeAlpha3MapByCountry.Add("Montserrat", "MSR");
            _countryCodeAlpha3MapByCountry.Add("Morocco", "MAR");
            _countryCodeAlpha3MapByCountry.Add("Mozambique", "MOZ");
            _countryCodeAlpha3MapByCountry.Add("Myanmar", "MMR");
            _countryCodeAlpha3MapByCountry.Add("Namibia", "NAM");
            _countryCodeAlpha3MapByCountry.Add("Nauru", "NRU");
            _countryCodeAlpha3MapByCountry.Add("Nepal", "NPL");
            _countryCodeAlpha3MapByCountry.Add("Netherlands", "NLD");
            _countryCodeAlpha3MapByCountry.Add("New Caledonia", "NCL");
            _countryCodeAlpha3MapByCountry.Add("New Zealand", "NZL");
            _countryCodeAlpha3MapByCountry.Add("Nicaragua", "NIC");
            _countryCodeAlpha3MapByCountry.Add("Niger", "NER");
            _countryCodeAlpha3MapByCountry.Add("Nigeria", "NGA");
            _countryCodeAlpha3MapByCountry.Add("Niue", "NIU");
            _countryCodeAlpha3MapByCountry.Add("Norfolk Island", "NFK");
            _countryCodeAlpha3MapByCountry.Add("North Macedonia", "MKD");
            _countryCodeAlpha3MapByCountry.Add("Northern Mariana Islands", "MNP");
            _countryCodeAlpha3MapByCountry.Add("Norway", "NOR");
            _countryCodeAlpha3MapByCountry.Add("Oman", "OMN");
            _countryCodeAlpha3MapByCountry.Add("Pakistan", "PAK");
            _countryCodeAlpha3MapByCountry.Add("Palau", "PLW");
            _countryCodeAlpha3MapByCountry.Add("Palestine, State of", "PSE");
            _countryCodeAlpha3MapByCountry.Add("Panama", "PAN");
            _countryCodeAlpha3MapByCountry.Add("Papua New Guinea", "PNG");
            _countryCodeAlpha3MapByCountry.Add("Paraguay", "PRY");
            _countryCodeAlpha3MapByCountry.Add("Peru", "PER");
            _countryCodeAlpha3MapByCountry.Add("Philippines", "PHL");
            _countryCodeAlpha3MapByCountry.Add("Pitcairn", "PCN");
            _countryCodeAlpha3MapByCountry.Add("Poland", "POL");
            _countryCodeAlpha3MapByCountry.Add("Portugal", "PRT");
            _countryCodeAlpha3MapByCountry.Add("Puerto Rico", "PRI");
            _countryCodeAlpha3MapByCountry.Add("Qatar", "QAT");
            _countryCodeAlpha3MapByCountry.Add("Réunion", "REU");
            _countryCodeAlpha3MapByCountry.Add("Romania", "ROU");
            _countryCodeAlpha3MapByCountry.Add("Russian Federation", "RUS");
            _countryCodeAlpha3MapByCountry.Add("Rwanda", "RWA");
            _countryCodeAlpha3MapByCountry.Add("Saint Barthélemy", "BLM");
            _countryCodeAlpha3MapByCountry.Add("Saint Helena, Ascension and Tristan da Cunha", "SHN");
            _countryCodeAlpha3MapByCountry.Add("Saint Kitts and Nevis", "KNA");
            _countryCodeAlpha3MapByCountry.Add("Saint Lucia", "LCA");
            _countryCodeAlpha3MapByCountry.Add("Saint Martin (French part)", "MAF");
            _countryCodeAlpha3MapByCountry.Add("Saint Pierre and Miquelon", "SPM");
            _countryCodeAlpha3MapByCountry.Add("Saint Vincent and the Grenadines", "VCT");
            _countryCodeAlpha3MapByCountry.Add("Samoa", "WSM");
            _countryCodeAlpha3MapByCountry.Add("San Marino", "SMR");
            _countryCodeAlpha3MapByCountry.Add("Sao Tome and Principe", "STP");
            _countryCodeAlpha3MapByCountry.Add("Saudi Arabia", "SAU");
            _countryCodeAlpha3MapByCountry.Add("Senegal", "SEN");
            _countryCodeAlpha3MapByCountry.Add("Serbia", "SRB");
            _countryCodeAlpha3MapByCountry.Add("Seychelles", "SYC");
            _countryCodeAlpha3MapByCountry.Add("Sierra Leone", "SLE");
            _countryCodeAlpha3MapByCountry.Add("Singapore", "SGP");
            _countryCodeAlpha3MapByCountry.Add("Sint Maarten (Dutch part)", "SXM");
            _countryCodeAlpha3MapByCountry.Add("Slovakia", "SVK");
            _countryCodeAlpha3MapByCountry.Add("Slovenia", "SVN");
            _countryCodeAlpha3MapByCountry.Add("Solomon Islands", "SLB");
            _countryCodeAlpha3MapByCountry.Add("Somalia", "SOM");
            _countryCodeAlpha3MapByCountry.Add("South Africa", "ZAF");
            _countryCodeAlpha3MapByCountry.Add("South Georgia and the South Sandwich Islands", "SGS");
            _countryCodeAlpha3MapByCountry.Add("South Sudan", "SSD");
            _countryCodeAlpha3MapByCountry.Add("Spain", "ESP");
            _countryCodeAlpha3MapByCountry.Add("Sri Lanka", "LKA");
            _countryCodeAlpha3MapByCountry.Add("Sudan", "SDN");
            _countryCodeAlpha3MapByCountry.Add("Suriname", "SUR");
            _countryCodeAlpha3MapByCountry.Add("Svalbard and Jan Mayen", "SJM");
            _countryCodeAlpha3MapByCountry.Add("Sweden", "SWE");
            _countryCodeAlpha3MapByCountry.Add("Switzerland", "CHE");
            _countryCodeAlpha3MapByCountry.Add("Syrian Arab Republic", "SYR");
            _countryCodeAlpha3MapByCountry.Add("Taiwan, Province of China", "TWN");
            _countryCodeAlpha3MapByCountry.Add("Tajikistan", "TJK");
            _countryCodeAlpha3MapByCountry.Add("Tanzania, United Republic of", "TZA");
            _countryCodeAlpha3MapByCountry.Add("Thailand", "THA");
            _countryCodeAlpha3MapByCountry.Add("Timor-Leste", "TLS");
            _countryCodeAlpha3MapByCountry.Add("Togo", "TGO");
            _countryCodeAlpha3MapByCountry.Add("Tokelau", "TKL");
            _countryCodeAlpha3MapByCountry.Add("Tonga", "TON");
            _countryCodeAlpha3MapByCountry.Add("Trinidad and Tobago", "TTO");
            _countryCodeAlpha3MapByCountry.Add("Tunisia", "TUN");
            _countryCodeAlpha3MapByCountry.Add("Turkey", "TUR");
            _countryCodeAlpha3MapByCountry.Add("Turkmenistan", "TKM");
            _countryCodeAlpha3MapByCountry.Add("Turks and Caicos Islands", "TCA");
            _countryCodeAlpha3MapByCountry.Add("Tuvalu", "TUV");
            _countryCodeAlpha3MapByCountry.Add("Uganda", "UGA");
            _countryCodeAlpha3MapByCountry.Add("Ukraine", "UKR");
            _countryCodeAlpha3MapByCountry.Add("United Arab Emirates", "ARE");
            _countryCodeAlpha3MapByCountry.Add("United Kingdom of Great Britain and Northern Ireland", "GBR");
            _countryCodeAlpha3MapByCountry.Add("United States of America", "USA");
            _countryCodeAlpha3MapByCountry.Add("United States Minor Outlying Islands", "UMI");
            _countryCodeAlpha3MapByCountry.Add("Uruguay", "URY");
            _countryCodeAlpha3MapByCountry.Add("Uzbekistan", "UZB");
            _countryCodeAlpha3MapByCountry.Add("Vanuatu", "VUT");
            _countryCodeAlpha3MapByCountry.Add("Venezuela (Bolivarian Republic of)", "VEN");
            _countryCodeAlpha3MapByCountry.Add("Vietnam", "VNM");
            _countryCodeAlpha3MapByCountry.Add("Virgin Islands (British)", "VGB");
            _countryCodeAlpha3MapByCountry.Add("Virgin Islands (U.S.)", "VIR");
            _countryCodeAlpha3MapByCountry.Add("Wallis and Futuna", "WLF");
            _countryCodeAlpha3MapByCountry.Add("Western Sahara", "ESH");
            _countryCodeAlpha3MapByCountry.Add("Yemen", "YEM");
            _countryCodeAlpha3MapByCountry.Add("Zambia", "ZMB");
            _countryCodeAlpha3MapByCountry.Add("Zimbabwe", "ZWE");

            // build the inverse
            _countryMapByAlpha3CountryCode = new Dictionary<string, string>();
            foreach (var country in _countryCodeAlpha3MapByCountry.Keys){
                _countryMapByAlpha3CountryCode.Add(_countryCodeAlpha3MapByCountry[country], country);
            }

            #endregion

            #region Alpha3ByNumeric - NumericByAlpha3
            _countryCodeAlpha3MapByNumeric = new Dictionary<string, string>();
            _countryCodeAlpha3MapByNumeric.Add("004", "AFG");
            _countryCodeAlpha3MapByNumeric.Add("248", "ALA");
            _countryCodeAlpha3MapByNumeric.Add("008", "ALB");
            _countryCodeAlpha3MapByNumeric.Add("012", "DZA");
            _countryCodeAlpha3MapByNumeric.Add("016", "ASM");
            _countryCodeAlpha3MapByNumeric.Add("020", "AND");
            _countryCodeAlpha3MapByNumeric.Add("024", "AGO");
            _countryCodeAlpha3MapByNumeric.Add("660", "AIA");
            _countryCodeAlpha3MapByNumeric.Add("010", "ATA");
            _countryCodeAlpha3MapByNumeric.Add("028", "ATG");
            _countryCodeAlpha3MapByNumeric.Add("032", "ARG");
            _countryCodeAlpha3MapByNumeric.Add("051", "ARM");
            _countryCodeAlpha3MapByNumeric.Add("533", "ABW");
            _countryCodeAlpha3MapByNumeric.Add("036", "AUS");
            _countryCodeAlpha3MapByNumeric.Add("040", "AUT");
            _countryCodeAlpha3MapByNumeric.Add("031", "AZE");
            _countryCodeAlpha3MapByNumeric.Add("044", "BHS");
            _countryCodeAlpha3MapByNumeric.Add("048", "BHR");
            _countryCodeAlpha3MapByNumeric.Add("050", "BGD");
            _countryCodeAlpha3MapByNumeric.Add("052", "BRB");
            _countryCodeAlpha3MapByNumeric.Add("112", "BLR");
            _countryCodeAlpha3MapByNumeric.Add("056", "BEL");
            _countryCodeAlpha3MapByNumeric.Add("084", "BLZ");
            _countryCodeAlpha3MapByNumeric.Add("204", "BEN");
            _countryCodeAlpha3MapByNumeric.Add("060", "BMU");
            _countryCodeAlpha3MapByNumeric.Add("064", "BTN");
            _countryCodeAlpha3MapByNumeric.Add("068", "BOL");
            _countryCodeAlpha3MapByNumeric.Add("535", "BES");
            _countryCodeAlpha3MapByNumeric.Add("070", "BIH");
            _countryCodeAlpha3MapByNumeric.Add("072", "BWA");
            _countryCodeAlpha3MapByNumeric.Add("074", "BVT");
            _countryCodeAlpha3MapByNumeric.Add("076", "BRA");
            _countryCodeAlpha3MapByNumeric.Add("086", "IOT");
            _countryCodeAlpha3MapByNumeric.Add("096", "BRN");
            _countryCodeAlpha3MapByNumeric.Add("100", "BGR");
            _countryCodeAlpha3MapByNumeric.Add("854", "BFA");
            _countryCodeAlpha3MapByNumeric.Add("108", "BDI");
            _countryCodeAlpha3MapByNumeric.Add("132", "CPV");
            _countryCodeAlpha3MapByNumeric.Add("116", "KHM");
            _countryCodeAlpha3MapByNumeric.Add("120", "CMR");
            _countryCodeAlpha3MapByNumeric.Add("124", "CAN");
            _countryCodeAlpha3MapByNumeric.Add("136", "CYM");
            _countryCodeAlpha3MapByNumeric.Add("140", "CAF");
            _countryCodeAlpha3MapByNumeric.Add("148", "TCD");
            _countryCodeAlpha3MapByNumeric.Add("152", "CHL");
            _countryCodeAlpha3MapByNumeric.Add("156", "CHN");
            _countryCodeAlpha3MapByNumeric.Add("162", "CXR");
            _countryCodeAlpha3MapByNumeric.Add("166", "CCK");
            _countryCodeAlpha3MapByNumeric.Add("170", "COL");
            _countryCodeAlpha3MapByNumeric.Add("174", "COM");
            _countryCodeAlpha3MapByNumeric.Add("178", "COG");
            _countryCodeAlpha3MapByNumeric.Add("180", "COD");
            _countryCodeAlpha3MapByNumeric.Add("184", "COK");
            _countryCodeAlpha3MapByNumeric.Add("188", "CRI");
            _countryCodeAlpha3MapByNumeric.Add("384", "CIV");
            _countryCodeAlpha3MapByNumeric.Add("191", "HRV");
            _countryCodeAlpha3MapByNumeric.Add("192", "CUB");
            _countryCodeAlpha3MapByNumeric.Add("531", "CUW");
            _countryCodeAlpha3MapByNumeric.Add("196", "CYP");
            _countryCodeAlpha3MapByNumeric.Add("203", "CZE");
            _countryCodeAlpha3MapByNumeric.Add("208", "DNK");
            _countryCodeAlpha3MapByNumeric.Add("262", "DJI");
            _countryCodeAlpha3MapByNumeric.Add("212", "DMA");
            _countryCodeAlpha3MapByNumeric.Add("214", "DOM");
            _countryCodeAlpha3MapByNumeric.Add("218", "ECU");
            _countryCodeAlpha3MapByNumeric.Add("818", "EGY");
            _countryCodeAlpha3MapByNumeric.Add("222", "SLV");
            _countryCodeAlpha3MapByNumeric.Add("226", "GNQ");
            _countryCodeAlpha3MapByNumeric.Add("232", "ERI");
            _countryCodeAlpha3MapByNumeric.Add("233", "EST");
            _countryCodeAlpha3MapByNumeric.Add("748", "SWZ");
            _countryCodeAlpha3MapByNumeric.Add("231", "ETH");
            _countryCodeAlpha3MapByNumeric.Add("238", "FLK");
            _countryCodeAlpha3MapByNumeric.Add("234", "FRO");
            _countryCodeAlpha3MapByNumeric.Add("242", "FJI");
            _countryCodeAlpha3MapByNumeric.Add("246", "FIN");
            _countryCodeAlpha3MapByNumeric.Add("250", "FRA");
            _countryCodeAlpha3MapByNumeric.Add("254", "GUF");
            _countryCodeAlpha3MapByNumeric.Add("258", "PYF");
            _countryCodeAlpha3MapByNumeric.Add("260", "ATF");
            _countryCodeAlpha3MapByNumeric.Add("266", "GAB");
            _countryCodeAlpha3MapByNumeric.Add("270", "GMB");
            _countryCodeAlpha3MapByNumeric.Add("268", "GEO");
            _countryCodeAlpha3MapByNumeric.Add("276", "DEU");
            _countryCodeAlpha3MapByNumeric.Add("288", "GHA");
            _countryCodeAlpha3MapByNumeric.Add("292", "GIB");
            _countryCodeAlpha3MapByNumeric.Add("300", "GRC");
            _countryCodeAlpha3MapByNumeric.Add("304", "GRL");
            _countryCodeAlpha3MapByNumeric.Add("308", "GRD");
            _countryCodeAlpha3MapByNumeric.Add("312", "GLP");
            _countryCodeAlpha3MapByNumeric.Add("316", "GUM");
            _countryCodeAlpha3MapByNumeric.Add("320", "GTM");
            _countryCodeAlpha3MapByNumeric.Add("831", "GGY");
            _countryCodeAlpha3MapByNumeric.Add("324", "GIN");
            _countryCodeAlpha3MapByNumeric.Add("624", "GNB");
            _countryCodeAlpha3MapByNumeric.Add("328", "GUY");
            _countryCodeAlpha3MapByNumeric.Add("332", "HTI");
            _countryCodeAlpha3MapByNumeric.Add("334", "HMD");
            _countryCodeAlpha3MapByNumeric.Add("336", "VAT");
            _countryCodeAlpha3MapByNumeric.Add("340", "HND");
            _countryCodeAlpha3MapByNumeric.Add("344", "HKG");
            _countryCodeAlpha3MapByNumeric.Add("348", "HUN");
            _countryCodeAlpha3MapByNumeric.Add("352", "ISL");
            _countryCodeAlpha3MapByNumeric.Add("356", "IND");
            _countryCodeAlpha3MapByNumeric.Add("360", "IDN");
            _countryCodeAlpha3MapByNumeric.Add("364", "IRN");
            _countryCodeAlpha3MapByNumeric.Add("368", "IRQ");
            _countryCodeAlpha3MapByNumeric.Add("372", "IRL");
            _countryCodeAlpha3MapByNumeric.Add("833", "IMN");
            _countryCodeAlpha3MapByNumeric.Add("376", "ISR");
            _countryCodeAlpha3MapByNumeric.Add("380", "ITA");
            _countryCodeAlpha3MapByNumeric.Add("388", "JAM");
            _countryCodeAlpha3MapByNumeric.Add("392", "JPN");
            _countryCodeAlpha3MapByNumeric.Add("832", "JEY");
            _countryCodeAlpha3MapByNumeric.Add("400", "JOR");
            _countryCodeAlpha3MapByNumeric.Add("398", "KAZ");
            _countryCodeAlpha3MapByNumeric.Add("404", "KEN");
            _countryCodeAlpha3MapByNumeric.Add("296", "KIR");
            _countryCodeAlpha3MapByNumeric.Add("408", "PRK");
            _countryCodeAlpha3MapByNumeric.Add("410", "KOR");
            _countryCodeAlpha3MapByNumeric.Add("414", "KWT");
            _countryCodeAlpha3MapByNumeric.Add("417", "KGZ");
            _countryCodeAlpha3MapByNumeric.Add("418", "LAO");
            _countryCodeAlpha3MapByNumeric.Add("428", "LVA");
            _countryCodeAlpha3MapByNumeric.Add("422", "LBN");
            _countryCodeAlpha3MapByNumeric.Add("426", "LSO");
            _countryCodeAlpha3MapByNumeric.Add("430", "LBR");
            _countryCodeAlpha3MapByNumeric.Add("434", "LBY");
            _countryCodeAlpha3MapByNumeric.Add("438", "LIE");
            _countryCodeAlpha3MapByNumeric.Add("440", "LTU");
            _countryCodeAlpha3MapByNumeric.Add("442", "LUX");
            _countryCodeAlpha3MapByNumeric.Add("446", "MAC");
            _countryCodeAlpha3MapByNumeric.Add("450", "MDG");
            _countryCodeAlpha3MapByNumeric.Add("454", "MWI");
            _countryCodeAlpha3MapByNumeric.Add("458", "MYS");
            _countryCodeAlpha3MapByNumeric.Add("462", "MDV");
            _countryCodeAlpha3MapByNumeric.Add("466", "MLI");
            _countryCodeAlpha3MapByNumeric.Add("470", "MLT");
            _countryCodeAlpha3MapByNumeric.Add("584", "MHL");
            _countryCodeAlpha3MapByNumeric.Add("474", "MTQ");
            _countryCodeAlpha3MapByNumeric.Add("478", "MRT");
            _countryCodeAlpha3MapByNumeric.Add("480", "MUS");
            _countryCodeAlpha3MapByNumeric.Add("175", "MYT");
            _countryCodeAlpha3MapByNumeric.Add("484", "MEX");
            _countryCodeAlpha3MapByNumeric.Add("583", "FSM");
            _countryCodeAlpha3MapByNumeric.Add("498", "MDA");
            _countryCodeAlpha3MapByNumeric.Add("492", "MCO");
            _countryCodeAlpha3MapByNumeric.Add("496", "MNG");
            _countryCodeAlpha3MapByNumeric.Add("499", "MNE");
            _countryCodeAlpha3MapByNumeric.Add("500", "MSR");
            _countryCodeAlpha3MapByNumeric.Add("504", "MAR");
            _countryCodeAlpha3MapByNumeric.Add("508", "MOZ");
            _countryCodeAlpha3MapByNumeric.Add("104", "MMR");
            _countryCodeAlpha3MapByNumeric.Add("516", "NAM");
            _countryCodeAlpha3MapByNumeric.Add("520", "NRU");
            _countryCodeAlpha3MapByNumeric.Add("524", "NPL");
            _countryCodeAlpha3MapByNumeric.Add("528", "NLD");
            _countryCodeAlpha3MapByNumeric.Add("540", "NCL");
            _countryCodeAlpha3MapByNumeric.Add("554", "NZL");
            _countryCodeAlpha3MapByNumeric.Add("558", "NIC");
            _countryCodeAlpha3MapByNumeric.Add("562", "NER");
            _countryCodeAlpha3MapByNumeric.Add("566", "NGA");
            _countryCodeAlpha3MapByNumeric.Add("570", "NIU");
            _countryCodeAlpha3MapByNumeric.Add("574", "NFK");
            _countryCodeAlpha3MapByNumeric.Add("807", "MKD");
            _countryCodeAlpha3MapByNumeric.Add("580", "MNP");
            _countryCodeAlpha3MapByNumeric.Add("578", "NOR");
            _countryCodeAlpha3MapByNumeric.Add("512", "OMN");
            _countryCodeAlpha3MapByNumeric.Add("586", "PAK");
            _countryCodeAlpha3MapByNumeric.Add("585", "PLW");
            _countryCodeAlpha3MapByNumeric.Add("275", "PSE");
            _countryCodeAlpha3MapByNumeric.Add("591", "PAN");
            _countryCodeAlpha3MapByNumeric.Add("598", "PNG");
            _countryCodeAlpha3MapByNumeric.Add("600", "PRY");
            _countryCodeAlpha3MapByNumeric.Add("604", "PER");
            _countryCodeAlpha3MapByNumeric.Add("608", "PHL");
            _countryCodeAlpha3MapByNumeric.Add("612", "PCN");
            _countryCodeAlpha3MapByNumeric.Add("616", "POL");
            _countryCodeAlpha3MapByNumeric.Add("620", "PRT");
            _countryCodeAlpha3MapByNumeric.Add("630", "PRI");
            _countryCodeAlpha3MapByNumeric.Add("634", "QAT");
            _countryCodeAlpha3MapByNumeric.Add("638", "REU");
            _countryCodeAlpha3MapByNumeric.Add("642", "ROU");
            _countryCodeAlpha3MapByNumeric.Add("643", "RUS");
            _countryCodeAlpha3MapByNumeric.Add("646", "RWA");
            _countryCodeAlpha3MapByNumeric.Add("652", "BLM");
            _countryCodeAlpha3MapByNumeric.Add("654", "SHN");
            _countryCodeAlpha3MapByNumeric.Add("659", "KNA");
            _countryCodeAlpha3MapByNumeric.Add("662", "LCA");
            _countryCodeAlpha3MapByNumeric.Add("663", "MAF");
            _countryCodeAlpha3MapByNumeric.Add("666", "SPM");
            _countryCodeAlpha3MapByNumeric.Add("670", "VCT");
            _countryCodeAlpha3MapByNumeric.Add("882", "WSM");
            _countryCodeAlpha3MapByNumeric.Add("674", "SMR");
            _countryCodeAlpha3MapByNumeric.Add("678", "STP");
            _countryCodeAlpha3MapByNumeric.Add("682", "SAU");
            _countryCodeAlpha3MapByNumeric.Add("686", "SEN");
            _countryCodeAlpha3MapByNumeric.Add("688", "SRB");
            _countryCodeAlpha3MapByNumeric.Add("690", "SYC");
            _countryCodeAlpha3MapByNumeric.Add("694", "SLE");
            _countryCodeAlpha3MapByNumeric.Add("702", "SGP");
            _countryCodeAlpha3MapByNumeric.Add("534", "SXM");
            _countryCodeAlpha3MapByNumeric.Add("703", "SVK");
            _countryCodeAlpha3MapByNumeric.Add("705", "SVN");
            _countryCodeAlpha3MapByNumeric.Add("090", "SLB");
            _countryCodeAlpha3MapByNumeric.Add("706", "SOM");
            _countryCodeAlpha3MapByNumeric.Add("710", "ZAF");
            _countryCodeAlpha3MapByNumeric.Add("239", "SGS");
            _countryCodeAlpha3MapByNumeric.Add("728", "SSD");
            _countryCodeAlpha3MapByNumeric.Add("724", "ESP");
            _countryCodeAlpha3MapByNumeric.Add("144", "LKA");
            _countryCodeAlpha3MapByNumeric.Add("729", "SDN");
            _countryCodeAlpha3MapByNumeric.Add("740", "SUR");
            _countryCodeAlpha3MapByNumeric.Add("744", "SJM");
            _countryCodeAlpha3MapByNumeric.Add("752", "SWE");
            _countryCodeAlpha3MapByNumeric.Add("756", "CHE");
            _countryCodeAlpha3MapByNumeric.Add("760", "SYR");
            _countryCodeAlpha3MapByNumeric.Add("158", "TWN");
            _countryCodeAlpha3MapByNumeric.Add("762", "TJK");
            _countryCodeAlpha3MapByNumeric.Add("834", "TZA");
            _countryCodeAlpha3MapByNumeric.Add("764", "THA");
            _countryCodeAlpha3MapByNumeric.Add("626", "TLS");
            _countryCodeAlpha3MapByNumeric.Add("768", "TGO");
            _countryCodeAlpha3MapByNumeric.Add("772", "TKL");
            _countryCodeAlpha3MapByNumeric.Add("776", "TON");
            _countryCodeAlpha3MapByNumeric.Add("780", "TTO");
            _countryCodeAlpha3MapByNumeric.Add("788", "TUN");
            _countryCodeAlpha3MapByNumeric.Add("792", "TUR");
            _countryCodeAlpha3MapByNumeric.Add("795", "TKM");
            _countryCodeAlpha3MapByNumeric.Add("796", "TCA");
            _countryCodeAlpha3MapByNumeric.Add("798", "TUV");
            _countryCodeAlpha3MapByNumeric.Add("800", "UGA");
            _countryCodeAlpha3MapByNumeric.Add("804", "UKR");
            _countryCodeAlpha3MapByNumeric.Add("784", "ARE");
            _countryCodeAlpha3MapByNumeric.Add("826", "GBR");
            _countryCodeAlpha3MapByNumeric.Add("840", "USA");
            _countryCodeAlpha3MapByNumeric.Add("581", "UMI");
            _countryCodeAlpha3MapByNumeric.Add("858", "URY");
            _countryCodeAlpha3MapByNumeric.Add("860", "UZB");
            _countryCodeAlpha3MapByNumeric.Add("548", "VUT");
            _countryCodeAlpha3MapByNumeric.Add("862", "VEN");
            _countryCodeAlpha3MapByNumeric.Add("704", "VNM");
            _countryCodeAlpha3MapByNumeric.Add("092", "VGB");
            _countryCodeAlpha3MapByNumeric.Add("850", "VIR");
            _countryCodeAlpha3MapByNumeric.Add("876", "WLF");
            _countryCodeAlpha3MapByNumeric.Add("732", "ESH");
            _countryCodeAlpha3MapByNumeric.Add("887", "YEM");
            _countryCodeAlpha3MapByNumeric.Add("894", "ZMB");
            _countryCodeAlpha3MapByNumeric.Add("716", "ZWE");                       

            // build the inverse
            _numericCodeMapByAlpha3CountryCode = new Dictionary<string, string>();
            foreach (var country in _countryCodeAlpha3MapByNumeric.Keys){
                _numericCodeMapByAlpha3CountryCode.Add(_countryCodeAlpha3MapByNumeric[country], country);
            }

            #endregion

            #endregion

        }
        /// <summary>
        /// Alpha2CodeByCountry
        /// </summary>       
        public Dictionary<string, string> Alpha2CodeByCountry {
            get
            {
                return _countryCodeAlpha2MapByCountry;
            }
        }
        /// <summary>
        /// CountryByAlpha2Code
        /// </summary>
        public Dictionary<string, string> CountryByAlpha2Code {
            get
            {
                return _countryMapByAlpha2CountryCode;
            }
        }
        /// <summary>
        /// Alpha3CodeByAlpha2Code
        /// </summary>
        public Dictionary<string, string> Alpha3CodeByAlpha2Code {
            get
            {
                return _countryCodeAlpha3MapAlpha2CountryCode;
            }
        }
        /// <summary>
        /// Alpha2CodeByAlpha3Code
        /// </summary>
        public Dictionary<string, string> Alpha2CodeByAlpha3Code {
            get
            {
                return _countryCodeAlpha2MapAlpha3CountryCode;
            }
        }
        /// <summary>
        /// Alpha3CodeByCountry
        /// </summary>       
        public Dictionary<string, string> Alpha3CodeByCountry {
            get
            {
                return _countryCodeAlpha3MapByCountry;
            }
        }
        /// <summary>
        /// CountryByAlpha3Code
        /// </summary>
        public Dictionary<string, string> CountryByAlpha3Code {
            get
            {
                return _countryMapByAlpha3CountryCode;
            }
        }
        /// <summary>
        /// CountryCodeByNumeric
        /// </summary>
        public Dictionary<string, string> Alpha3CountryCodeByNumeric {
            get
            {
                return _countryCodeAlpha3MapByNumeric;
            }
        }
        /// <summary>
        /// NumericByCountryCode
        /// </summary>
        public Dictionary<string, string> NumericByAlpha3CountryCode {
            get
            {
                return _numericCodeMapByAlpha3CountryCode;
            }
        }
        /// <summary>
        /// CountryCodeByNumeric
        /// </summary>
        public Dictionary<string, string> Alpha2CountryCodeByNumeric {
            get
            {
                return _countryCodeAlpha2MapByNumericCode;
            }
        }
        /// <summary>
        /// NumericByCountryCode
        /// </summary>
        public Dictionary<string, string> NumericByAlpha2CountryCode {
            get
            {
                return _numericCodeMapByCountryCodeAlpha2;
            }
        }
        /// <summary>
        /// NumericCodeByCountry
        /// </summary>
        public Dictionary<string, string> NumericCodeByCountry {
            get
            {
                return _numericCodeMapByCountry;
            }
        }
        /// <summary>
        /// CountryByNumericCode
        /// </summary>
        public Dictionary<string, string> CountryByNumericCode {
            get
            {
                return _countryMapByNumeric;
            }
        }
    }
}
