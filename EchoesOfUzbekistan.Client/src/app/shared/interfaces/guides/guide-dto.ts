export interface GuideDto {
    id: string;
    title: string;
    description?: string;
    city: string;
    priceAmount: number;
    priceCurrency: string;
    status: string;
    datePublished: string; 
    dateEdited?: string; 
    authorId: string;
    originalLanguageId: string;
    audioLink?: string;
    imageLink?: string;
    audioUrl?: string;
    imageUrl?: string;
}
