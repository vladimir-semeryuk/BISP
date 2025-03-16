export interface GuidePostDto {
  title: string;
  description?: string;
  city: string;
  moneyAmount: number;
  currencyCode: string;
  languageCode: string;
  authorId: string;
  datePublished: string; 
  audioLink?: string;
  imageLink?: string;
  placeIds?: string[];
}
