import EventoForm from "@/components/EventoForm";

export default function Home() {
  return (
    <main className="flex min-h-screen items-center justify-center bg-[radial-gradient(circle_at_top,#d1fae5,transparent_30%),linear-gradient(180deg,#f8fafc_0%,#eef2ff_100%)] px-4 py-12 sm:px-6 lg:px-8">
      <EventoForm />
    </main>
  );
}
