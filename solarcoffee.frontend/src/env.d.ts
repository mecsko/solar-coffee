import "vite/client";

interface ImportMetaEnv extends Readonly<Record<string, string | boolean | undefined>> {
  VITE_API_URL_HTTP: string;
  VITE_API_URL_HTTPS: string;
}

interface ImportMeta {
  env: ImportMetaEnv;
}
