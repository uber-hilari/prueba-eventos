import type { Evento, EventoErrors } from "./types";
import ErrorMessage from "./ErrorMessage";
import { getInputClass, toDateInput } from "./utils";

type EventoFieldsProps = {
  evento: Evento;
  errors: EventoErrors;
  onFieldBlur: (field: keyof Omit<Evento, "zonas">, rawValue: string) => void;
};

export default function EventoFields({
  evento,
  errors,
  onFieldBlur,
}: EventoFieldsProps) {
  return (
    <>
      <div className="grid gap-6 sm:grid-cols-2">
        <label className="space-y-2 text-sm font-medium text-zinc-800">
          <span>Nombre</span>
          <input
            type="text"
            name="nombre"
            defaultValue={evento.nombre}
            onBlur={(event) => onFieldBlur("nombre", event.target.value)}
            className={getInputClass(Boolean(errors.nombre))}
          />
          <ErrorMessage error={errors.nombre} />
        </label>

        <label className="space-y-2 text-sm font-medium text-zinc-800">
          <span>Fecha</span>
          <input
            type="date"
            name="fecha"
            defaultValue={toDateInput(evento.fecha)}
            onBlur={(event) => onFieldBlur("fecha", event.target.value)}
            className={getInputClass(Boolean(errors.fecha))}
          />
          <ErrorMessage error={errors.fecha} />
        </label>
      </div>

      <label className="space-y-2 text-sm font-medium text-zinc-800">
        <span>Lugar</span>
        <input
          type="text"
          name="lugar"
          defaultValue={evento.lugar}
          onBlur={(event) => onFieldBlur("lugar", event.target.value)}
          className={getInputClass(Boolean(errors.lugar))}
        />
        <ErrorMessage error={errors.lugar} />
      </label>
    </>
  );
}
