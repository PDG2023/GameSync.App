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
