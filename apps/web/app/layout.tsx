import type { Metadata } from "next";
import "./globals.css";

export const metadata: Metadata = {
  title: "Drakoda — AI Media Studio",
  description: "Create images, video and audio with one AI creative workspace."
};

export default function RootLayout({ children }: Readonly<{ children: React.ReactNode }>) {
  return <html lang="en"><body>{children}</body></html>;
}
