import axios from "axios";

export const ApiClient = axios.create({
  baseURL:
    process.env.NODE_ENV === "production"
      ? "http://www.ecoservices.pk"
      : "http://localhost:5000"
});
