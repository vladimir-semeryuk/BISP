export interface PlacePost {
    title: string;
    description?: string;
    longitude: number;
    latitude: number;
    languageCode: string;
    authorId: string;
    audioLink?: string;
    imageLink?: string;
}
