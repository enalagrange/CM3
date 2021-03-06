﻿// Concept Matrix 3.
// Licensed under the MIT license.

namespace ConceptMatrix.Injection.Memory
{
	using System;
	using System.Collections.Generic;

	public abstract class MemoryBase
	{
		protected ProcessInjection process;
		protected UIntPtr address;

		private static List<MemoryBase> activeMemory = new List<MemoryBase>();

		public MemoryBase(ProcessInjection process, UIntPtr address)
		{
			this.process = process;
			this.address = address;

			lock (activeMemory)
			{
				activeMemory.Add(this);
			}

			this.Active = true;
		}

		public bool Active
		{
			get;
			private set;
		}

		public string Name { get; set; }

		public static void TickAllActiveMemory()
		{
			List<MemoryBase> memories;
			lock (activeMemory)
			{
				memories = new List<MemoryBase>(activeMemory);
			}

			foreach (MemoryBase memory in memories)
			{
				if (!InjectionService.ProcessIsAlive)
					return;

				memory.Tick();
			}
		}

		public virtual void Dispose()
		{
			lock (activeMemory)
			{
				activeMemory.Remove(this);
			}

			this.Active = false;
		}

		protected abstract void Tick();
	}
}
