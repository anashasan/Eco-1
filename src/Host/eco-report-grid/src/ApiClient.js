import axios from "axios";

export const ApiClient = axios.create({
  baseURL:
    process.env.NODE_ENV === "production"
      ? "http://35.200.190.55:8080"
      : "http://localhost:5000"
});
