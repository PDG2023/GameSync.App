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
  items: GameSearchResultItem[],
  nextPage: string;
  previousPage: string;
}

export interface GameSearchResultItem {
  yearPublished: number;
  name: string;
  isExpansion: boolean;
  id: number;
  thumbnailUrl: string;
  imageUrl: string;
}

export interface Game {
  name: string;
  minPlayer: number;
  maxPlayer: number;
  minAge: number;
  description: string;
  durationMinute: number;
}

export interface GameDetail extends Game {
  id: number;
}

export interface GameList extends GameDetail {
  userId: string;
}
