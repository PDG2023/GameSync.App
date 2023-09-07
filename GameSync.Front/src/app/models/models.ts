
export interface Me {
  email: string;
  userName: string;
}

export interface User extends Me {
  password: string;
  token?: string;
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
  count: number;
}

export interface Game {
  id: number;
  name: string;
  yearPublished?: number;
  isExpansion?: boolean;
  thumbnailUrl?: string;
  imageUrl?: string;
}

export interface GameDetail extends Game {
  minPlayer: number;
  maxPlayer: number;
  minAge?: number;
  description: string;
  durationMinute?: number;
}

export interface GameCollectionItem extends Game {
  isCustom?: boolean;
}

export interface GameDetailResult {
  game: GameDetail;
  inCollection: boolean;
}

export interface GameVoteInfo {
  id: number;
  gameImageUrl: string;
  gameName: string;
  whoVotedYes: string[];
  countVotedYes: number;
  whoVotedNo: string[];
  countVotedNo: number;
}

export interface BaseParty {
  id: number;
  location: string;
  name: string;
  dateTime: Date;
}

export interface Party extends BaseParty {
  numberOfGames: 0;
}

export interface PartyDetail extends BaseParty {
  isOwner: boolean;
  gamesVoteInfo: GameVoteInfo[];
}

export interface PartyDetailRequest {
  id: number;
  invitationToken?: string;
}

export interface PartyGameRequest {
  games: PartyGameRequestItem[]
}

export interface PartyGameRequestItem {
  id: number;
  isCustom?: boolean;
}

export interface PartyGameDetail {
  game: GameDetail;
  isCustom: boolean;
}

export interface VoteInfo {
  userName: string;
  voteYes: boolean | null;
}

export interface VoteRequest {
  invitationToken?: string;
  partyId?: string;
  partyGameId: number;
  voteInfo: VoteInfo;
}
