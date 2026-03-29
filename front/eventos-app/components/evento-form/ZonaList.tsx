import type { Zona, ZonaErrors } from "./types";
import ErrorMessage from "./ErrorMessage";
import { getInputClass } from "./utils";

type ZonaListProps = {
  zonas: Zona[];
  errors: ZonaErrors[];
  onAddZona: () => void;
  onRemoveZona: (index: number) => void;
  onZonaBlur: (index: number, field: keyof Zona, rawValue: string) => void;
};

export default function ZonaList({
  zonas,
  errors,
  onAddZona,
  onRemoveZona,
  onZonaBlur,
}: ZonaListProps) {
  return (
    <div className="space-y-4">
      <div className="flex flex-col gap-3 sm:flex-row sm:items-center sm:justify-between">
        <div>
          <h2 className="text-lg font-semibold text-zinc-950">Zonas</h2>
          <p className="text-sm text-zinc-500">
            Cada zona debe incluir nombre, precio y capacidad.
          </p>
        </div>
        <button
          type="button"
          onClick={onAddZona}
          className="rounded-full border border-emerald-200 bg-emerald-50 px-4 py-2 text-sm font-medium text-emerald-700 transition hover:border-emerald-300 hover:bg-emerald-100"
        >
          Agregar zona
        </button>
      </div>

      <div className="space-y-4">
        {zonas.map((zona, index) => (
          <div
            key={index}
            className="rounded-2xl border border-zinc-200 bg-zinc-50/80 p-4"
          >
            <div className="mb-4 flex items-center justify-between gap-3">
              <h3 className="text-sm font-semibold uppercase tracking-[0.2em] text-zinc-500">
                Zona {index + 1}
              </h3>
              <button
                type="button"
                onClick={() => onRemoveZona(index)}
                disabled={zonas.length === 1}
                className="text-sm font-medium text-rose-600 transition hover:text-rose-700 disabled:cursor-not-allowed disabled:text-zinc-300"
              >
                Eliminar
              </button>
            </div>

            <div className="grid gap-4 sm:grid-cols-[minmax(0,1fr)_140px_140px]">
              <label className="space-y-2 text-sm font-medium text-zinc-800">
                <span>Nombre</span>
                <input
                  type="text"
                  defaultValue={zona.nombre}
                  onBlur={(event) =>
                    onZonaBlur(index, "nombre", event.target.value)
                  }
                  className={getInputClass(Boolean(errors[index]?.nombre), "white")}
                />
                <ErrorMessage error={errors[index]?.nombre} />
              </label>

              <label className="space-y-2 text-sm font-medium text-zinc-800">
                <span>Precio</span>
                <input
                  type="number"
                  min="0"
                  step="0.01"
                  defaultValue={zona.precio}
                  onBlur={(event) =>
                    onZonaBlur(index, "precio", event.target.value)
                  }
                  className={getInputClass(Boolean(errors[index]?.precio), "white")}
                />
                <ErrorMessage error={errors[index]?.precio} />
              </label>

              <label className="space-y-2 text-sm font-medium text-zinc-800">
                <span>Capacidad</span>
                <input
                  type="number"
                  min="0"
                  step="1"
                  defaultValue={zona.capacidad}
                  onBlur={(event) =>
                    onZonaBlur(index, "capacidad", event.target.value)
                  }
                  className={getInputClass(
                    Boolean(errors[index]?.capacidad),
                    "white",
                  )}
                />
                <ErrorMessage error={errors[index]?.capacidad} />
              </label>
            </div>
          </div>
        ))}
      </div>
    </div>
  );
}
