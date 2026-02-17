export interface HealthResponse {
  status: "ok";
  service: string;
  env: "development" | "test" | "production";
}
