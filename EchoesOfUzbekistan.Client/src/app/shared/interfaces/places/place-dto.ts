export interface PlaceDto {
    title: string;
    description?: string;
    coordinates: string; // GEOJSON 
    status: string; // TODO: REPLACE WITH ENUM
    datePublished: string; // TODO: Maybe it's worth using some Date data type
    dateEdited?: string; // same as above
    authorId: string; // Guid as string
    originalLanguageId: string;
    audioLink?: string | null; 
    imageLink?: string | null;
    translations: PlaceTranslationDto[];
  }
  
  export interface PlaceTranslationDto {
    languageId: string;
    placeTitle: string;
    placeDescription?: string;
    audioLink?: string | null;
    placeId: string;
  }