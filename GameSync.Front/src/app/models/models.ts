export interface User {
  email: string;
  userName: string | null;
  password: string | null;
  token: string | null;
}

export interface HttpErrorResponseDetail {
  code: number;
  description: string;
}

export interface GameSearchRequest {
  query: string;
  pageSize: number;
  page: number;
}

export interface GameSearchResult {
  items: Game[],
  nextPage: string;
  previousPage: string;
}

export interface Game {
  id: number;
  name: string;
  yearPublished: number;
  isExpansion?: boolean;
  thumbnailUrl?: string;
  imageUrl?: string;
}

export interface GameDetail extends Game {
  minPlayer: number;
  maxPlayer: number;
  minAge: number;
  description: string;
  durationMinute: number;
}

export interface GameCollection extends GameDetail {
  userId: string;
}
