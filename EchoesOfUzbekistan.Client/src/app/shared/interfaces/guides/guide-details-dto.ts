import { PlaceDto } from "../places/place-dto";
import { GuideTranslationDto } from "./guide-translation-dto";

export interface GuideDetailsDto {
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
  places: PlaceDto[];
  translations: GuideTranslationDto[];
  likeCount: number;
  authorName: string;
}
