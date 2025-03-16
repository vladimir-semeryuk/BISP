export interface UserDetails {
    id: string;
    firstName: string;
    surname: string;
    email: string;
    registrationDateUtc: string; 
    countryName: string;
    countryCode: string;
    city?: string;
    aboutMe?: string;
    // roles?: string[]; // TODO
  }
  
  